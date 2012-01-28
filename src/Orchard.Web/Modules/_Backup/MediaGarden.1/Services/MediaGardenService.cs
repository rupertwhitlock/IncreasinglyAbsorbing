using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml.Linq;

using Orchard;
using Orchard.Data;
using Orchard.Environment;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.DisplayManagement;
using Orchard.UI.Notify;
using Orchard.FileSystems.Media;
using Orchard.Utility.Extensions;

using Downplay.Origami;

using MediaGarden.Pipeline;
using MediaGarden.Models;
using MediaGarden.ViewModels;


namespace MediaGarden.Services
{
    public class MediaGardenService : IMediaGardenService
    {
        public IOrchardServices Services { get; set; }

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IStorageProvider _storageProvider;

        private readonly Lazy<IEnumerable<IMediaFormat>> _mediaFormats;
        private readonly Lazy<IEnumerable<IMediaViewer>> _mediaViewers;

        private readonly Lazy<IEnumerable<IMediaQueryFilter>> _mediaQueryFilters;
        private readonly Lazy<IEnumerable<IMediaLocationFilter>> _mediaLocationFilters;
        private readonly Lazy<IEnumerable<IMediaHeaderFilter>> _mediaHeaderFilters;
        private readonly Lazy<IEnumerable<IMediaSourceFilter>> _mediaSourceFilters;
        private readonly Lazy<IEnumerable<IMediaContentFilter>> _mediaContentFilters;
        private readonly Lazy<IEnumerable<IMediaViewerFilter>> _mediaViewerFilters;

        private readonly IRepository<MediaSourceRecord> _mediaSourceRepository;
        private readonly IRepository<MediaSessionRecord> _mediaSessionRepository;

        private readonly IMapper<XElement, SettingsDictionary> _settingsReader;
        private readonly IMapper<SettingsDictionary, XElement> _settingsWriter;

        /// <summary>
        /// TODO: Either use lazies, or split this class up into separate services, for performance purposes
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mediaFormats"></param>
        /// <param name="mediaViewers"></param>
        /// <param name="mediaInputs"></param>
        /// <param name="mediaQueryFilters"></param>
        /// <param name="mediaLocationFilters"></param>
        /// <param name="mediaHeaderFilters"></param>
        /// <param name="mediaSourceFilters"></param>
        /// <param name="mediaViewerFilters"></param>
        /// <param name="mediaSourceRepository"></param>
        /// <param name="mediaSessionRepository"></param>
        /// <param name="settingsReader"></param>
        /// <param name="settingsWriter"></param>
        /// <param name="shapeFactory"></param>
        public MediaGardenService(IOrchardServices services,
            IContentDefinitionManager contentDefinitionManager,
            IStorageProvider storageProvider,
            Lazy<IEnumerable<IMediaFormat>> mediaFormats, 
            Lazy<IEnumerable<IMediaViewer>> mediaViewers,
            Lazy<IEnumerable<IMediaQueryFilter>> mediaQueryFilters,
            Lazy<IEnumerable<IMediaLocationFilter>> mediaLocationFilters,
            Lazy<IEnumerable<IMediaHeaderFilter>> mediaHeaderFilters,
            Lazy<IEnumerable<IMediaSourceFilter>> mediaSourceFilters,
            Lazy<IEnumerable<IMediaContentFilter>> mediaContentFilters,
            Lazy<IEnumerable<IMediaViewerFilter>> mediaViewerFilters,
            IRepository<MediaSourceRecord> mediaSourceRepository,
            IRepository<MediaSessionRecord> mediaSessionRepository,
            IMapper<XElement, SettingsDictionary> settingsReader,
            IMapper<SettingsDictionary, XElement> settingsWriter,
            IShapeFactory shapeFactory) {

            Services = services;
            _contentDefinitionManager = contentDefinitionManager;
            _storageProvider = storageProvider;
            _mediaFormats = mediaFormats;
            _mediaViewers = mediaViewers;
            _mediaQueryFilters = mediaQueryFilters;
            _mediaLocationFilters = mediaLocationFilters;
            _mediaSourceFilters = mediaSourceFilters;
            _mediaContentFilters = mediaContentFilters;
            _mediaHeaderFilters = mediaHeaderFilters;
            _mediaViewerFilters = mediaViewerFilters;
            _mediaSourceRepository = mediaSourceRepository;
            _mediaSessionRepository = mediaSessionRepository;
            _settingsReader = settingsReader;
            _settingsWriter = settingsWriter;
            Shape = shapeFactory;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        dynamic Shape { get; set; }

        public IEnumerable<String> AllStereotypes()
        {
            return AllMediaTypes()
                .Select(t => t.Settings["MediaStereotype"]).Distinct().OrderBy(t => t);
        }

        /// <summary>
        /// Get everything with media stereotype
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentTypeDefinition> AllMediaTypes()
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd => ctd.Settings.ContainsKey("MediaStereotype") && ctd.Parts.Any(p => p.PartDefinition.Name == "MediaPart")).OrderBy(p=>p.DisplayName);
        }

        public MediaSessionRecord BeginMediaSession()
        {
            // Location could be anything like: external URL to a media file, feed or HTML page; internal folder or filename; other arbitrarily defined location conventions
            var record = new MediaSessionRecord()
            {
//                Query = location
            };
            _mediaSessionRepository.Create(record);
            return record;
        }
        public IQueryable<MediaSourceRecord> LoadMediaSession(int mediaSessionId)
        {
            var session = _mediaSessionRepository.Get(mediaSessionId);
            if (session == null) return null;
            return _mediaSourceRepository.Table.Where(source => source.SessionRecord.Id == mediaSessionId);
        }

        public IMediaSource MediaSourceFromRecord(MediaSourceRecord source)
        {
            return new MediaSource(source)
            {
                Metadata = _settingsReader.Map(Parse(source.Metadata))
            };
        }
        public IMediaSource GetSource(int id)
        {
            var record = _mediaSourceRepository.Get(id);
            if (record == null) return null;
            return MediaSourceFromRecord(record);
        }

        public IMediaItem MediaPreviewItemFromSource(IMediaSource source)
        {
            var item = new MediaPreviewItem(source)
            {
                MediaFormat = MatchFormats(source).First()
            };
            item.MediaViewer = FindViewer(item,(item.MediaSource!=null)?item.MediaSource.Record.ViewerName:null);
            return item;
        }


        public IEnumerable<MediaSourceContext> Pull(MediaSessionRecord session, string query, string mediaStereotype="")
        {
            // Perform location filter
            var context = new MediaQueryContext() {
                MediaStereotype=mediaStereotype,
                Query = query
            };
            foreach (var qf in _mediaQueryFilters.Value)
            {
                qf.QueryFiltering(context);
            }

            // Context now contains some locations
            foreach (var location in context.Locations) {
                foreach (var lf in _mediaLocationFilters.Value)
                {
                    lf.LocationFiltering(context, location);
                }
            }

            // Context now contains headers
            var sourceList = new List<MediaSourceContext>();
            foreach (var hf in _mediaHeaderFilters.Value)
            {
                foreach (var header in context.Headers)
                {
                    hf.HeaderFiltering(context, header);

                    foreach(var item in context.Sources) {
                        // Source filters build metadata straight away
                        // TODO: It's all a bit inflexible and unclear what can be done at each stage of pipeline; need to organise and clarify things a bit
                        var formats = MatchFormats(item,context.MediaStereotype);
                        var filters = _mediaSourceFilters.Value.Where(f => f.SupportedFormats().Any(id => id=="*" || formats.Any(format => format.FormatName == id)));
                        foreach (var sf in filters)
                        {
                            sf.BuildSourceMetadata(context, header.LocationContext, item);
                        }
                        sourceList.Add(item);
                    }
                    context.Sources.Clear();
                }
            }

            // Context now contains sources; save them
            var resultList = new List<MediaSourceContext>();
            foreach (var source in sourceList)
            {
                var item = source.Source;
                var record =
                    new MediaSourceRecord
                    {
                        Location = item.Url,
                        SessionRecord = session,
                        MediaStereotype = item.Record.MediaStereotype,
                        FormatName = item.Record.FormatName,
                        LastModified = item.Record.LastModified
                    };
                Apply(item, record);
                _mediaSourceRepository.Create(record);
                resultList.Add(new MediaSourceContext(MediaSourceFromRecord(record)){
                    Stream = source.Stream
                });
            }

            // Finish query filters, clean up
            foreach (var f in _mediaQueryFilters.Value)
            {
                f.QueryFiltered(context);
            }
            return resultList;
        }

        public IEnumerable<Orchard.ContentManagement.IContent> Pick(IEnumerable<IMediaSource> sources, bool asDrafts = false)
        {
            List<IContent> results = new List<IContent>();
            foreach (var source in sources)
            {
                var filterContext = new MediaContentContext()
                {
                    Source = source
                };
                // Match format(s) ... is there really the potential for multiples or should we just take the first and/or raise an error for ambiguities?
                var formats = MatchFormats(source,source.MediaStereotype);
                filterContext.Formats = formats;

                var filters = _mediaSourceFilters.Value.Where(f => f.SupportedFormats().Any(id => id == "*" || formats.Any(format => format.FormatName == id)));

                foreach (var filter in filters)
                {
                    filter.SourceFiltering(filterContext, source);
                }

                // Create each item
                foreach (var create in filterContext.Creators)
                {
                    // Create cam be cancelled by setting ContentType=null
                    if (String.IsNullOrWhiteSpace(create.ContentType)) continue;

                    // Create item
                    var content = Services.ContentManager.New(create.ContentType);
                    create.ContentItem = content;

                    content.With<MediaPart>(part =>
                    {
                        part.Source = source.Record;
                        // TODO: Will error if no matching formats. On the other hand we shouldn't even have any items without a valid format.
                        part.FormatName = formats.FirstOrDefault().FormatName;
                    });

                    // Perform content filtering
                    foreach (var contentFilter in _mediaContentFilters.Value) {
                        contentFilter.ContentFiltering(create);
                    }

                    // Stop create
                    if (create.Cancel == true) continue;

                    // Draft or publish
                    if (asDrafts)
                    {
                        Services.ContentManager.Create(content, VersionOptions.Draft);
                    }
                    else
                    {
                        Services.ContentManager.Create(content, VersionOptions.Published);
                    }

                    filterContext.Items.Add(content);

                    // Final content filter
                    foreach (var contentFilter in _mediaContentFilters.Value)
                    {
                        contentFilter.ContentFiltered(create);
                    }

                }

                if (filterContext.Processed && filterContext.Items.Any())
                {
                    foreach (var filter in filters)
                    {
                        filter.SourceFiltered(filterContext, source);
                    }
                }

                // Finally notify and return the list
                foreach (var item in filterContext.Items)
                {
                    // TODO: Is DisplayName localized? (Note: this applies elsewhere)
                    // TODO: Hmm - ways to support localization of item title at this stage?
                    Services.Notifier.Information(T("Created {0} media item '<a href=\"{2}\">{1}</a>'", item.TypeDefinition.DisplayName, item.GetTitle(),Null.Url.AbsoluteDisplayUrl(item)));
                    results.Add(item);
                }

                results.AddRange(filterContext.Items);
            }
            return results;
        }
        /// <summary>
        /// Preferred method if we already have a context (saves building the context and finding stream accessors, etc.)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<IMediaFormat> MatchFormats(MediaSourceContext source, string mediaStereotype = "")
        {
            return StereotypedFormats(mediaStereotype).Where(f => f.IsOfFormat(source));
        }

        public IEnumerable<IMediaFormat> MatchFormats(IMediaSource source,string mediaStereotype="")
        {
            return MatchFormats(BuildMediaSourceContext(source),mediaStereotype);
        }
        public IMediaFormat GetFormat(string formatName)
        {
            // TODO: Optimise with keyed dictionary? - Depends how often this is called but will be at least once per media view
            // TODO: Will throw an error if same named format exists twice, should test and handle this (and maybe apply module/feature names to IDs of formats and other relevant bits)
            return _mediaFormats.Value.Where(f => f.FormatName == formatName).First();
        }

        public IMediaViewer FindViewer(IMediaItem item, string overrideName = null)
        {
            // Determine compatible viewers
            var viewers = MatchViewers(item);

            var context = new MediaViewerContext()
            {
                Content = item as IContent,
                MediaItem = item,
                Viewers = viewers,
                ViewerName = overrideName
            };
            
            var ordered = viewers.OrderByDescending(f => f.ViewerPriority(context));

            foreach (var filter in _mediaViewerFilters.Value)
            {
                filter.ViewerSelecting(context);
            }

            // Take override if available
            if (!String.IsNullOrWhiteSpace(context.ViewerName))
            {
                var viewer = ordered.FirstOrDefault(mv => mv.ViewerName == overrideName);
                if (viewer != null) context.Viewer = viewer;
            }
            else
            {
                context.Viewer = ordered.FirstOrDefault();
            }

            foreach (var filter in _mediaViewerFilters.Value)
            {
                filter.ViewerSelected(context);
            }

            // Take first matching viewer
            // TODO: A nice way to prioritise viewers by specificity (although that will still happen somewhat via shape alternates)
            return context.Viewer;
        }


        public IEnumerable<IMediaViewer> MatchViewers(IMediaItem item)
        {
            // Determine compatible viewers
            var format = item.MediaFormat;
            var viewers = _mediaViewers.Value.Where(mv => mv.SupportedMediaFormats().Any(f => f == "*" || f == format.FormatName));
            return viewers;
        }

        private MediaSourceContext BuildMediaSourceContext(IMediaSource source)
        {
            var context = new MediaSourceContext(source);
            context.Stream = FindStreamAccessor(source);
            return context;
        }

        private IStreamAccessor FindStreamAccessor(IMediaSource source)
        {
            // TODO: This feels slightly hackish, we're abusing the Query Filters to pull out the
            //       Stream. This could be doubly bad because we could potentially end up with a WebRequest stream
            //       for local files. Should separate out the stream accessor and make it a function of the Location filter perhaps.
            //       Finally, not 100% sure how this will work and how we should be tidying up the open streams at the end.

            var context = new MediaQueryContext()
            {
                Query = source.Url
            };
            foreach (var qf in _mediaQueryFilters.Value)
            {
                qf.QueryFiltering(context);
            }
            foreach (var qf in _mediaQueryFilters.Value)
            {
                qf.QueryFiltered(context);
            }
            if (!context.Locations.Any()) return null;
            return context.Locations.First().Stream;
        }
        

        /// <summary>
        /// Get formats of a particular media stereotype
        /// TODO: Support "Mixed" option here?
        /// </summary>
        /// <param name="mediaStereotype"></param>
        /// <returns></returns>
        public IEnumerable<IMediaFormat> StereotypedFormats(string mediaStereotype)
        {
            if (String.IsNullOrWhiteSpace(mediaStereotype)) return _mediaFormats.Value;
            return _mediaFormats.Value.Where(f => f.MediaStereotype == mediaStereotype);
        }

        private void Apply(IMediaSource model, MediaSourceRecord record)
        {
            record.Metadata = Compose(_settingsWriter.Map(model.Metadata));
        }

        XElement Parse(string settings)
        {
            if (string.IsNullOrEmpty(settings))
                return null;

            try
            {
                return XElement.Parse(settings);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to parse media source metadata xml");
                return null;
            }
        }

        static string Compose(XElement map)
        {
            if (map == null)
                return null;

            return map.ToString();
        }

        public string AbsoluteMediaUrl(IMediaItem media)
        {
            if (media.MediaUrl.LastIndexOf(':') < 0)
            {
                var url = Null.Url;
                // TODO: Could refactor this so location filters are responsible for determining the absolute media URL? This way we can have more than one type of local location.
                return url.RequestContext.HttpContext.Request.ToRootUrlString() + _storageProvider.GetPublicUrl(media.MediaUrl);
            }
            else
            {
                return media.MediaUrl;
            }
        }
    }
}
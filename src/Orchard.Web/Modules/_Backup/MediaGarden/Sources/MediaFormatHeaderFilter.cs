using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;
using MediaGarden.Pipeline;
using MediaGarden.Services;
using Orchard.Environment;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Notify;
namespace MediaGarden.Sources
{
    /// <summary>
    /// This source looks at available formats to see if any already match the location itself.
    /// So this handles entering a URL directly to an item, or pulling in a feed location as a playlist in itself (rather than parsing to find individual media items)
    /// Other header filters will be responsible for actually reading in a stream to determine any further data
    /// </summary>
    public class MediaFormatHeaderFilter : IMediaHeaderFilter
    {
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        private readonly Work<IMediaGardenService> _gardenService;

        public string HeaderName
        {
            get { return "MediaFormat"; }
        }

        public MediaFormatHeaderFilter(
            IOrchardServices _orchardServices,
            Work<IMediaGardenService> gardenService)
        {
            Services = _orchardServices;
            _gardenService = gardenService;
            T= NullLocalizer.Instance;
        }

        public void HeaderFiltering(MediaQueryContext query, MediaHeaderContext header)
        {
            // Build a temporary source to test formats
            var testSource = new MediaSource()
            {
                Url = header.LocationContext.Location,
                TimeStamp = header.TimeStamp
            };
            testSource.Metadata["ContentLength"] = header.ContentLength.ToString();
            testSource.Metadata["MimeType"] = header.ContentType;
            testSource.Metadata["Title"] = header.Title;
            testSource.Metadata["SourceName"] = HeaderName;
            var testContext = new MediaSourceContext(testSource)
                {
                    Stream = header.LocationContext.Stream
                };
            var formats = _gardenService.Value.MatchFormats(testContext,query.MediaStereotype).ToList();
            // Match a format, add it to sources
            if (formats.Any())
            {
                testContext.Formats = formats;
                var format = formats.First();
                testSource.MediaStereotype = format.MediaStereotype;
                testSource.FormatName = format.FormatName;
                query.Sources.Add(testContext);
            }
            else
            {
                // Helpful message for debugging sources that don't work :)
                Services.Notifier.Warning(T("No matching media formats found at {0} (with MIME type '{1}' and file extension '{2}')",
                    testSource.Url,
                    testSource.Metadata["MimeType"],
                    testSource.FileExtension()
                    ));
            }
        }

        public void HeaderFiltered(MediaQueryContext query, MediaHeaderContext header)
        {
        }
    }
}
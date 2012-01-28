using System;
using System.Linq;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Orchard;
using Orchard.Data;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.DisplayManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Settings;
using Orchard.Core.Contents.Controllers;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Media.Services;

using MediaGarden.Models;
using MediaGarden.Services;
using MediaGarden.ViewModels;
using Orchard.UI.Zones;
using ClaySharp.Implementation;
using Orchard.Mvc;
using MediaGarden.Pipeline;
using Downplay.Origami.Services;

namespace MediaGarden.Controllers {
    public class AdminController : Controller, IUpdateModel
    {
        
        public IOrchardServices Services { get; private set; }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ISiteService _siteService;
        private readonly IMediaService _mediaService;
        private readonly IMediaGardenService _gardenService;
        private readonly IOrigamiService _origamiService;
        dynamic Shape { get; set; }
        private readonly IShapeFactory _shapeFactory;

        public AdminController(
                IOrchardServices services,
                IContentManager contentManager,
                IContentDefinitionManager contentDefinitionManager,
                ITransactionManager transactionManager,
                ISiteService siteService,
                IShapeFactory shapeFactory,
                IMediaService mediaService,
                IMediaGardenService gardenService,
                IOrigamiService origamiService
        ){
            Services = services;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _transactionManager = transactionManager;
            _siteService = siteService;
            _mediaService = mediaService;
            _gardenService = gardenService;
            _origamiService = origamiService;
            Shape = _shapeFactory = shapeFactory;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }
        
        /// <summary>
        /// This action is where we can start importing media from sources
        /// TODO: I'd like to move a lot of code out of this controller so it's not tied to the admin page (for instance, easily accessible in content editors, media picker, etc.)
        /// Maybe it could be a partial view, or even handle everything within Shape code.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pagerParameters"></param>
        /// <returns></returns>
        public ActionResult Sources(ListSourcesViewModel model, PagerParameters pagerParameters)
        {
            return SourcesResult(model, pagerParameters, false);
        }
        [HttpPost,ActionName("Sources")]
        public ActionResult SourcesPOST(ListSourcesViewModel model, PagerParameters pagerParameters)
        {
            return SourcesResult(model, pagerParameters, true);
        }

        protected ActionResult SourcesResult(ListSourcesViewModel model, PagerParameters pagerParameters, bool update)
        {
            // Security
            if (!Services.Authorizer.Authorize(Orchard.Media.Permissions.ManageMedia, T("Couldn't list media")))
                return new HttpUnauthorizedResult();

            // TODO: Copied from AdminController in Core.Contents; but we're not ensuring that this is a "Media" type as nor was
            // Core.Contents ensuring it was a Creatable type... should raise a workitem for there as well?
            if (!string.IsNullOrEmpty(model.TypeName))
            {
                var foundDef = _contentDefinitionManager.ListTypeDefinitions().Any(
                    ctd => ctd.Settings.Has("MediaStereotype", model.TypeName));
                if (!foundDef)
                    return HttpNotFound();
            }

            var sources = BuildSourcesUI(model, pagerParameters, update);

            // Session Id in query string
            var routeValues = ControllerContext.RouteData.Values;
            routeValues["mediaSessionId"] = sources.Model.MediaSessionId;
            // Redirect on success
            if (update && this.ModelState.IsValid) {
                return RedirectToAction("Sources", routeValues);
            }

            // TODO: Check context for other types of result we might need
            return new ShapeResult(this, sources.Shape);
        }

        /// <summary>
        /// Builds the whole sources page composition
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pagerParameters"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        protected MediaSourcesViewModelContext BuildSourcesUI(ListSourcesViewModel model, PagerParameters pagerParameters, bool update)
        {
            // Pager
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            // Build query for media sources
            IQueryable<MediaSourceRecord> query;

            // Try to load a session if there is one
            if (model.MediaSessionId.HasValue)
            {
                query = _gardenService.LoadMediaSession(model.MediaSessionId.Value);
                if (query == null)
                {
                    Services.Notifier.Error(T("Media session does not exist"));
                    query = Enumerable.Empty<MediaSourceRecord>().AsQueryable();
                }
            }
            else
            {
                query = Enumerable.Empty<MediaSourceRecord>().AsQueryable();
            }

            if (!string.IsNullOrEmpty(model.TypeName))
            {
                model.TypeDisplayName = T(model.TypeName).Text;
                query = query.Where(d => d.MediaStereotype == model.TypeName);
            }

            // TODO: Some media-specific orderings
            query = query.OrderByDescending(dr => dr.LastModified);
            /*
            switch (model.Options.OrderBy)
            {
                case ContentsOrder.Modified:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.ContentItemRecord.Versions.Single(civr => civr.Latest).Id);
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.ModifiedUtc);
                    break;
                case ContentsOrder.Published:
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.PublishedUtc);
                    break;
                case ContentsOrder.Created:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.Id);
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.CreatedUtc);
                    break;
            }*/

            // TODO: Following filter is performed thru admin menu instead, don't really need it 
            model.Options.SelectedFilter = model.TypeName;
            model.Options.FilterOptions = _gardenService.AllMediaTypes()
                .Select(ctd => new KeyValuePair<string, string>(ctd.Name, ctd.DisplayName))
                .ToList();

            var pagerShape = Shape.Pager(pager).TotalItemCount(query.Count());
            var pageOfSourceData = query.Skip(pager.GetStartIndex()).Take(pager.PageSize).ToList();

            // TODO: Add zone holding in a shape event instead
            var zoneHoldingBehavior = new ZoneHoldingBehavior(() => _shapeFactory.Create("ContentZone", Arguments.Empty()));
            dynamic sources = _shapeFactory.Create("Media_Sources", Arguments.Empty(), new[] { zoneHoldingBehavior });
            sources
//                .MediaSources(list)
                .Pager(pagerShape)
                .Options(model.Options)
                .TypeDisplayName(model.TypeDisplayName ?? "");

            var sourcesModel = new MediaSourcesViewModel()
            {
                MediaSessionId = model.MediaSessionId,
                MediaStereotype = model.Id,
                Sources = pageOfSourceData.Select(s=>{
                    var source = _gardenService.MediaSourceFromRecord(s);
                    return new MediaSourceViewModel(source){
                        Preview = _gardenService.MediaPreviewItemFromSource(source)
                    };
                }).ToList()
            };

            _origamiService.BuildEditorShape(sourcesModel, sources, update?this:null, "Media", "Detail", "MediaSource");

            var resultContext = new MediaSourcesViewModelContext()
            {
                Shape = sources,
                Model = sourcesModel
            };

            return resultContext;
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return base.TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

    }
}

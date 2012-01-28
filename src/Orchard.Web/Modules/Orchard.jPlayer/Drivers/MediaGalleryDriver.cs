using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.jPlayer.Models;
using Orchard.jPlayer.Models.Plugins;
using Orchard.jPlayer.Services;
using Orchard.jPlayer.ViewModels;
using Orchard.UI.Resources;

namespace Orchard.jPlayer.Drivers {
    public class MediaGalleryDriver : ContentPartDriver<MediaGalleryPart> {
        private readonly IMediaGalleryService _mediaGalleryService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public MediaGalleryDriver(IMediaGalleryService mediaGalleryService, IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;
            _mediaGalleryService = mediaGalleryService;
        }

        private void RegisterStaticContent(TypeResourceDescriptor pluginResourceDescriptor) {
            IResourceManager resourceManager = _workContextAccessor.GetContext().Resolve<IResourceManager>();

            foreach(string script in pluginResourceDescriptor.Scripts) {
                resourceManager.RegisterHeadScript(script);
            }

            foreach(LinkEntry style in pluginResourceDescriptor.Styles) {
                resourceManager.RegisterLink(style);
            }

            resourceManager.Require("script", "jQuery").AtHead();
        }

        protected override DriverResult Display(MediaGalleryPart part, string displayType, dynamic shapeHelper) {
            TypeFactory typeFactory = TypeFactory.GetFactory(part.SelectedType);
            Models.MediaGallery mediaGallery = _mediaGalleryService.GetMediaGallery(part.MediaGalleryName);

            if(displayType == "SummaryAdmin") {
                return null;
            }

            if(string.IsNullOrWhiteSpace(part.MediaGalleryName))
                return null;

            RegisterStaticContent(typeFactory.TypeResourceDescriptor);

            MediaGalleryViewModel viewModel = new MediaGalleryViewModel { MediaGalleryType = typeFactory.Type };
            viewModel.Medias = mediaGallery.Medias;
            viewModel.AutoPlay = mediaGallery.AutoPlay;

            return ContentShape("Parts_MediaGallery",
                                () => shapeHelper.DisplayTemplate(
                                    TemplateName: typeFactory.Type.MediaGalleryTemplateName,
                                    Model: viewModel,
                                    Prefix: Prefix));
        }

        //GET
        protected override DriverResult Editor(
            MediaGalleryPart part, dynamic shapeHelper) {
            part.AvailableGalleries = _mediaGalleryService.GetMediaGalleries()
                .OrderBy(o => o.Name).Select(o => new SelectListItem {
                    Text = o.Name,
                    Value = o.Name
                });

            if(!string.IsNullOrWhiteSpace(part.MediaGalleryName)) {
                part.SelectedGallery = part.MediaGalleryName;
            }
            else {
                part.SelectedGallery = part.AvailableGalleries.FirstOrDefault() == null
                                           ? string.Empty
                                           : part.AvailableGalleries.FirstOrDefault().Value;
            }

            part.AvailableTypes = Enum.GetNames(typeof(MediaType))
                .Select(o => new SelectListItem {
                    Text = o,
                    Value = o
                });

            return ContentShape("Parts_MediaGallery_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/MediaGallery",
                                    Model: part,
                                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            MediaGalleryPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            part.MediaGalleryName = part.SelectedGallery;
            return Editor(part, shapeHelper);
        }
    }
}
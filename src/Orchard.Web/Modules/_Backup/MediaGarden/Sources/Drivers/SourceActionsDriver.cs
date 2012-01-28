using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.ViewModels;
using MediaGarden.Sources.ViewModels;
using Downplay.Origami.Services;
using MediaGarden.Models;
using MediaGarden.Services;
using Orchard;

namespace MediaGarden.Sources.Drivers
{
    /// <summary>
    /// This produces the important buttons on the sources page
    /// </summary>
    public class SourceActionsDriver : ModelDriver<MediaSourcesViewModel,SourceCommandsModel>
    {
        private readonly IMediaGardenService _gardenService;

        public SourceActionsDriver(IOrchardServices services, IMediaGardenService gardenService)
        {
            Services = services;
            _gardenService = gardenService;
        }

        public IOrchardServices Services { get; set; }
 
        public override SourceCommandsModel ViewModel(MediaSourcesViewModel model, ModelShapeContext context)
        {
            return new SourceCommandsModel();
        }

        protected override string Prefix
        {
            get { return "Commands"; }
        }

        protected override ModelDriverResult Update(MediaSourcesViewModel model, SourceCommandsModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                updater.TryUpdateModel(viewModel, prefix, null, null);
                if (!String.IsNullOrWhiteSpace(viewModel.CommandName))
                {
                    context.OnUpdated((updated) =>
                    {
                        switch (viewModel.CommandName)
                        {
                            case "ImportAll":
                                PerformImport(model.Sources);
                                break;
                            case "ImportSelected":
                                PerformImport(model.Sources.Where(s => s.Selected));
                                break;
                            case "PlaylistSelected":
                                break;
                            case "Refresh":
                                break;
                        }
                    });
                }
            }
            return EditorShape("Media_Actions_Sources", viewModel, context);
        }

        protected void PerformImport(IEnumerable<MediaSourceViewModel> sources)
        {
            var results = _gardenService.Pick(sources.Select(s=>s.Source));

            // TODO: Following previously from Controller, need to do some kind of redirect

            /*
            // routeValues["mediaSessionId"] = mediaSessionId;
            // Redirect to content admin
            routeValues["area"] = "Contents";
            // Reset stereotype in route.
            // TODO: Could transfer to appropriate ContentType of what's been created.
            // Maybe should just bring back media list screen to display all content imported from a session.
            routeValues["id"] = "";*/
        }

    }
}
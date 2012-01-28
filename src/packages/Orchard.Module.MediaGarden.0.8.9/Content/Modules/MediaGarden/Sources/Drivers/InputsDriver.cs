using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Orchard.Localization;
using MediaGarden.Services;
using MediaGarden.ViewModels;

namespace MediaGarden.Sources
{
    /// <summary>
    /// 
    /// </summary>
    public class InputsDriver : ModelDriver<MediaSourcesViewModel, MediaInputsViewModel>
    {
        private readonly IMediaGardenService _gardenService;

        public InputsDriver(IMediaGardenService gardenService)
        {
            _gardenService = gardenService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override MediaInputsViewModel ViewModel(MediaSourcesViewModel model, ModelShapeContext context)
        {
            return model.Inputs;
        }

        protected override ModelDriverResult Update(MediaSourcesViewModel model, MediaInputsViewModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                if (!updater.TryUpdateModel(viewModel, prefix, null,null))
                {
//                    updater.AddModelError(prefix+".Fetch", T
                }
            }
            // Chain to another origami process for anything that wants to handle the viewModel.
            return Combined(
                // Fetch button
                // TODO: Would be nice to have something like a button shape result for easily adding and handling buttons
                ModelShape("Media_Inputs_Fetch", () => shapeHelper.EditorTemplate(TemplateName: "Media.Inputs.Fetch", Model: viewModel, Prefix: prefix)),
                ChainShape(viewModel, prefix, () =>
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.Fetch))
                    {
                        var result = viewModel.Queries;
                        // Perform input fetch
                        if (!result.Any()) {
                            // TODO: Invididual inputs could raise errors ...?
                            updater.AddModelError(prefix+".Fetch",T("You must enter a location, either as a Url or a local file path, * for all"));
                            
                            // TODO: How to trigger a redirect from here...
                            // return RedirectToAction("Sources", routeValues);
                        }
                        else {
                            var session = _gardenService.BeginMediaSession();
                            // TODO: Hmm... only able to return the last session. A session perhaps needs to contain multiple locations.
                            foreach (var query in result)
                            {
                                // Let media garden pull in the media sources (will be saved straightaway to DB)
                                var results = _gardenService.Pull(session, query, model.MediaStereotype);
                                // Plant session Id
                                model.MediaSessionId = session.Id;
                            }
                        }
                    }
                })
                );
        }

        protected override string Prefix
        {
            get { return "MediaInputs"; }
        }
    }
}
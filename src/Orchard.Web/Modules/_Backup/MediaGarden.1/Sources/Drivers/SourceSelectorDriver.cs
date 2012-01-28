using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using MediaGarden.Sources.ViewModels;
using MediaGarden.ViewModels;

namespace MediaGarden.Sources.Drivers
{
    /// <summary>
    /// Produces the selector for source rows
    /// TODO: Selector could be a more core part of Origami, it's something we likely want any time we're generating a bunch of edit rows. Could implement model metadata in
    /// the model context - add additional metadata there relating to the model without having to create a wrapper like MediaSourceViewModel every time...
    /// </summary>
    public class SourceSelectorDriver : MediaSourceDriver<SourceSelectorViewModel>
    {
        protected override string Prefix
        {
            get { return "Selector"; }
        }

        public override SourceSelectorViewModel ViewModel(MediaSourceViewModel model, Downplay.Origami.Services.ModelShapeContext context)
        {
            return new SourceSelectorViewModel() { Selected = model.Selected };
        }

        protected override Downplay.Origami.Services.ModelDriverResult Update(MediaSourceViewModel model, SourceSelectorViewModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, Downplay.Origami.Services.ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                // Select in model for processing at the end
                updater.TryUpdateModel(viewModel, prefix, null, null);
                if (viewModel.Selected)
                {
                    model.Selected = true;
                }
            }
            return EditorShape("Media_Source_Selector", viewModel, context);
        }

    }
}
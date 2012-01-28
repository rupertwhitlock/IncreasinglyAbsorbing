using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami;
using MediaGarden.Pipeline;
using Orchard.ContentManagement;
using MediaGarden.Models;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;
using MediaGarden.Sources.ViewModels;
using Downplay.Mechanics.Plumbing.Models;
using Orchard.Core.Routable.Models;

namespace MediaGarden.Sources.Drivers
{
    public class TitleDriver : MediaSourceDriver<TitleModel>
    {

        protected override string Prefix
        {
            get { return "Title"; }
        }

        public override TitleModel ViewModel(MediaSourceViewModel model, ModelShapeContext context)
        {
            return new TitleModel()
            {
                Title = model.Source.Title
            };
        }

        protected override ModelDriverResult Update(MediaSourceViewModel source, TitleModel viewModel, dynamic shapeHelper, IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                if (updater.TryUpdateModel<TitleModel>(viewModel, prefix, null, null))
                {
                    // Save to source record
                    source.Source.Title = viewModel.Title;
                }
            }
            return EditorShape("Media_Source_Title",viewModel,context);
        }
    }
}
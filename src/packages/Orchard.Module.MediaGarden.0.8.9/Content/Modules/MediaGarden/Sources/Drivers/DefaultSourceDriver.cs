using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using MediaGarden.Models;
using MediaGarden.ViewModels;

namespace MediaGarden.Drivers
{
    /// <summary>
    /// Drives all the standard media source shapes
    /// </summary>
    public class DefaultSourceDriver : ModelDriver<MediaSourceViewModel>
    {
        protected override string Prefix
        {
            get { return "Source"; }
        }

        protected override ModelDriverResult Display(MediaSourceViewModel model, dynamic shapeHelper, ModelDisplayShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(MediaSourceViewModel model, dynamic shapeHelper, ModelEditorShapeContext context)
        {
            return Update(model, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(MediaSourceViewModel model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);

            if (updater != null)
            {
                if (updater.TryUpdateModel(model,prefix,null,null))
                {
                    // Possibly handle stereotype, format etc. changed
                }
            }

            return Combined(
                ModelShape("Media_Source_Url", shapeHelper.Media_Source_Url(MediaItem: model.Preview)),
                ModelShape("Media_Source_Preview",shapeHelper.Media_Source_Preview(MediaItem:model.Preview))
            );
        }
    }
}
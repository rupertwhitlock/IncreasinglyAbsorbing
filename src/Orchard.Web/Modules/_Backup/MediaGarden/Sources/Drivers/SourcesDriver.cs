using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.ViewModels;
using Downplay.Origami.Services;
using MediaGarden.Models;
using MediaGarden.Services;

namespace MediaGarden.Sources.Drivers
{
    /// <summary>
    /// Renders each source record shape
    /// </summary>
    public class SourcesDriver : ModelDriver<MediaSourcesViewModel>
    {

        private readonly IMediaGardenService _gardenService;

        public SourcesDriver(
            IMediaGardenService gardenService
            )
        {
            _gardenService = gardenService;
        }
        
        protected override string Prefix
        {
            get { return "Sources"; }
        }

        protected override ModelDriverResult Display(MediaSourcesViewModel model, dynamic shapeHelper, ModelDisplayShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(MediaSourcesViewModel model, dynamic shapeHelper, ModelEditorShapeContext context)
        {
            return Update(model, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(MediaSourcesViewModel model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var results = new List<ModelDriverResult>();
            var prefix = FullPrefix(context);
            foreach (var source in model.Sources)
            {
                var sourcePrefix = prefix+"."+source.Source.RecordId;
                var sourceShape = shapeHelper.Media_Source(
                    MediaSource:source.Source,
                    MediaItem: source.Preview
                );
                // Allow child shape to be built
                results.Add(ChildShape("Media_Source", sourceShape, source, sourcePrefix, "Summary"));
                // Insert into shape tree
                results.Add(ModelShape("Media_Source",source.Source.MediaStereotype, sourceShape));
            }
            return Combined(results.ToArray());
        }

    }
}
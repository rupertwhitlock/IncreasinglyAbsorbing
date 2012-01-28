using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Orchard.ContentManagement;
using MediaGarden.Models;
using Orchard.UI.Zones;
using MediaGarden.Services;

namespace MediaGarden.Shapes
{
    public class Shapes : IShapeTableProvider
    {
        private readonly Lazy<IMediaGardenService> _garden;

        public Shapes(
            Lazy<IMediaGardenService> garden
            )
        {
            _garden = garden;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            // Media View
            builder.Describe("Media")
                .OnCreating(creating=>{})
                .OnDisplaying(displaying=>{
                    IMediaItem media = displaying.Shape.MediaItem;
                    // TODO: Give up displaying the shape if we don't have details(?)
                    // OR... do some of this in other events so we can populate missing data. Then it's also possible to build the shape without having a media item, but with the stereotypes etc.
                    if (media != null)
                    {
                        // Populate url and title
                        displaying.Shape.MediaUrl = _garden.Value.AbsoluteMediaUrl(media);
                        displaying.Shape.Title = media.Title;

                        // Populate alternates for the media type
                        String mediaStereoType = media.MediaStereotype;
                        String mediaFormat = media.MediaFormat.FormatName;
                        String viewerName = media.MediaViewer.ViewerName;

                        // Media_Image.cshtml
                        displaying.ShapeMetadata.Alternates.Add("Media_" + mediaStereoType);
                        // Media_Jpeg.cshtml
                        displaying.ShapeMetadata.Alternates.Add("Media_" + mediaFormat);
                        // Media_Image_Jpeg.cshtml
                        displaying.ShapeMetadata.Alternates.Add("Media_" + mediaStereoType + "_" + mediaFormat);
                        // Add additional alternates is a viewer name is specified
                        if (!String.IsNullOrWhiteSpace(viewerName))
                        {
                            displaying.ShapeMetadata.Alternates.Add("Media__" + viewerName);
                            foreach (var alt in displaying.ShapeMetadata.Alternates.ToList())
                            {
                                displaying.ShapeMetadata.Alternates.Add(alt + "__" + viewerName);
                            }
                        }

                        // TODO: Above alternates should be in addition to other override factors such as DisplayType, Zone, etc. Check if this happens automatically.
                    }
                });

            // Give Zone Holding behavior to media source shape so all the UI bits work
            builder.Describe("Media_Source")
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.New.Zone())));

        }
    }
}
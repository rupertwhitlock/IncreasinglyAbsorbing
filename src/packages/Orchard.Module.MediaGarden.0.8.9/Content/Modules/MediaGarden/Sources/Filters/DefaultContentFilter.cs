using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using Orchard.ContentManagement;
using MediaGarden.Models;
using Downplay.Mechanics.Plumbing.Models;
using Orchard.Core.Routable.Models;

namespace MediaGarden.Sources.Filters
{
    public class DefaultContentFilter : IMediaContentFilter
    {

        public void ContentFiltering(MediaCreateContext context)
        {

            // Load up some standard values if parts and metadata are available
            var item = context.ContentItem;
            var source = context.Source;

            item.With<TitlePart>(t =>
            {
                t.Title = source.Title ?? source.Url;
            });

            item.With<RoutePart>(r =>
            {
                r.Title = source.Title ?? source.Url;
            });

            item.With<FileSizePart>(f =>
            {
                if (source.Metadata.ContainsKey("ContentLength"))
                {
                    f.Size = source.Metadata["ContentLength"].ParseLong() ?? 0;
                }
            });

            item.With<PixelDimensionsPart>(p =>
            {
                if (source.Metadata.ContainsKey("SizeX"))
                {
                    p.SizeX = source.Metadata["SizeX"].ParseInt() ?? 0;
                }
                if (source.Metadata.ContainsKey("SizeY"))
                {
                    p.SizeY = source.Metadata["SizeY"].ParseInt() ?? 0;
                }
            });

            // TODO: To support pixel dimensions for video, as well as audio/video media length, we'd need to do complex server-side processing. OR this data can come from the original
            // feed that sources came from, OR we have have the Flash player supply values from the Preview if we write a special player to handle that ...

        }

        public void ContentFiltered(MediaCreateContext context)
        {
        }

    }
}
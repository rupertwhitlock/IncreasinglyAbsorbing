using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;
using Orchard;

namespace MediaGarden.Pipeline
{
    public interface IMediaViewer : IDependency
    {
        String ViewerName { get; }
        String ViewerDescription { get; }
        int ViewerPriority(MediaViewerContext context);
        /// <summary>
        /// Gets a list of broadly-support formats
        /// </summary>
        /// <returns></returns>
        IEnumerable<String> SupportedMediaFormats();
    }
}

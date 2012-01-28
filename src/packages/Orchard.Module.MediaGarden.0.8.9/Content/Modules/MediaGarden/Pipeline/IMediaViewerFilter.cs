using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// Allows code selection of media viewers
    /// </summary>
    public interface IMediaViewerFilter : IDependency
    {
        void ViewerSelecting(MediaViewerContext context);
        void ViewerSelected(MediaViewerContext context);
    }
}

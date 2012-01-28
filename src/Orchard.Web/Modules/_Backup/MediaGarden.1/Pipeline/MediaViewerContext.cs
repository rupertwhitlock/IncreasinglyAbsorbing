using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    public class MediaViewerContext
    {
        public IContent Content { get; set; }
        public IMediaItem MediaItem { get; set; }

        public IEnumerable<IMediaViewer> Viewers { get; set; }

        public string ViewerName { get; set; }

        public IMediaViewer Viewer { get; set; }
    }
}

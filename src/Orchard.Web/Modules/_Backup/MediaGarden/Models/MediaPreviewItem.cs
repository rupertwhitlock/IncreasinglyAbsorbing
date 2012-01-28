using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaGarden.Models
{
    public class MediaPreviewItem : IMediaItem
    {
        public MediaPreviewItem(IMediaSource source)
        {
            MediaSource = source;
        }

        public string Title
        {
            get { return "Preview"; }
        }

        public string MediaUrl { get { return MediaSource.Url; } }

        public string MediaStereotype { get { return MediaFormat.MediaStereotype; } }

        public Pipeline.IMediaFormat MediaFormat { get; set; }

        public IMediaSource MediaSource { get; set; }

        public Pipeline.IMediaViewer MediaViewer { get; set; }
    }
}
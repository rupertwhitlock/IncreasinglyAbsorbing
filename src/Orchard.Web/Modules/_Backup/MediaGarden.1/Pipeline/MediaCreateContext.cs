using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;
using Orchard.ContentManagement;

namespace MediaGarden.Pipeline
{
    public class MediaCreateContext
    {

        public MediaCreateContext(MediaContentContext context, IMediaSource source)
        {
            ContentContext = context;
            Source = source;
            Cancel = false;
        }

        public MediaContentContext ContentContext { get; set; }

        public string ContentType { get; set; }

        public ContentItem ContentItem { get; set; }

        public IMediaSource Source { get; set; }

        public bool Cancel { get; set; }
    }
}

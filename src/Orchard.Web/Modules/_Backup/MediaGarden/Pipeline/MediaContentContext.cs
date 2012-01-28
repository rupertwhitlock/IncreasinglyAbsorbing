using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;
using Orchard.ContentManagement;

namespace MediaGarden.Pipeline
{
    public class MediaContentContext
    {

        public MediaContentContext() {
            Items = new List<ContentItem>();
            Creators = new List<MediaCreateContext>();
        }

        public bool Processed = false;
        public IMediaSource Source { get; set; }
        public IEnumerable<IMediaFormat> Formats { get; set; }
        public ICollection<ContentItem> Items { get; protected set; }
        public ICollection<MediaCreateContext> Creators { get; protected set; }
    }
}

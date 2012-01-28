using System;

namespace Orchard.jPlayer.Models {
    public class MediaGalleryMedia {
        public string Name { get; set; }

        public string Title { get; set; }

        public DateTime LastUpdated { get; set; }

        public int Position { get; set; }

        public string User { get; set; }

        public long Size { get; set; }

        public string PublicUrl { get; set; }
    }
}
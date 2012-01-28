using System;
using System.Collections.Generic;

namespace Orchard.jPlayer.Models {
    public class MediaGallery {
        public MediaGallery() {
            Medias = new List<MediaGalleryMedia>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string MediaPath { get; set; }

        public string User { get; set; }

        public DateTime LastUpdated { get; set; }

        public long Size { get; set; }

        public bool AutoPlay { get; set; }

        public IEnumerable<MediaGalleryMedia> Medias { get; set; }
    }
}
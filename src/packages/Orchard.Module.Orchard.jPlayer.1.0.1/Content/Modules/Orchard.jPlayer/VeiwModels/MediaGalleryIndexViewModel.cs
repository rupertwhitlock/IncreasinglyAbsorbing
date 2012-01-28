using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.jPlayer.ViewModels {
    public class MediaGalleryIndexViewModel {
        public MediaGalleryIndexViewModel() {
            MediaGalleries = new List<Models.MediaGallery>();
        }

        public IEnumerable<Models.MediaGallery> MediaGalleries { get; set; }
    }
}
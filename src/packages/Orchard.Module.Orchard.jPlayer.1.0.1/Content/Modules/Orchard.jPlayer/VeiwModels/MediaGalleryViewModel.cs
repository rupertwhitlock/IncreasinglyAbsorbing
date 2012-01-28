using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.jPlayer.Models;
using Orchard.jPlayer.Models.Plugins;

namespace Orchard.jPlayer.ViewModels {
    public class MediaGalleryViewModel {
        public IEnumerable<MediaGalleryMedia> Medias { get; set; }

        public bool AutoPlay { get; set; }

        public MediaGalleryType MediaGalleryType { get; set; }
    }
}
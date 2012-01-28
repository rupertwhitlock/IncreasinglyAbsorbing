using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.jPlayer.Models;

namespace Orchard.jPlayer.ViewModels {
    public class MediaGalleryMediasViewModel {
        public IEnumerable<MediaGalleryMedia> Medias { get; set; }

        public string MediaGalleryName { get; set; }

        public string GripIconPublicUrl { get; set; }
    }
}
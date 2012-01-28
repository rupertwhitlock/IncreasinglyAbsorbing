using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.jPlayer.Models;

namespace Orchard.jPlayer.ViewModels {
    public class MediaEditViewModel {
        public string MediaGalleryName { get; set; }

        public MediaGalleryMedia Media { get; set; }
    }
}
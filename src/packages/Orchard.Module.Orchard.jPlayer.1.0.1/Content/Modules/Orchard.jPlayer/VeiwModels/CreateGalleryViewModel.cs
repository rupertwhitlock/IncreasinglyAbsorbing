using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orchard.jPlayer.ViewModels {
    public class CreateGalleryViewModel {
        [Required]
        public string GalleryName { get; set; }
    }
}
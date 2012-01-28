using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.jPlayer.ViewModels {
    public class MediaAddViewModel {
        public string MediaGalleryName { get; set; }

        public IEnumerable<HttpPostedFileBase> MediaFiles { get; set; }
    }
}
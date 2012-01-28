using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Orchard.jPlayer.Models {
    public class MediaGalleryRecord : ContentPartRecord {
        public virtual bool AutoPlay { get; set; }

        public virtual string MediaGalleryName { get; set; }

        public virtual byte MediaType { get; set; }
    }
}
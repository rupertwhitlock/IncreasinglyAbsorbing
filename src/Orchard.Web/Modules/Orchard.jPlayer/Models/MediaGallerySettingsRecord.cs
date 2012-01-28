using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.jPlayer.Models {
    public class MediaGallerySettingsRecord {
        public virtual int Id { get; set; }

        public virtual string MediaGalleryName { get; set; }

        public virtual IList<MediaGalleryMediaSettingsRecord> MediaSettings { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.jPlayer.Models {
    public class MediaGalleryMediaSettingsRecord {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Title { get; set; }

        public virtual int Position { get; set; }
    }
}
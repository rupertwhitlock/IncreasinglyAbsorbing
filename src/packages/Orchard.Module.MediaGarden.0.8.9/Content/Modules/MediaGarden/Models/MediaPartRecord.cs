using Orchard;
using Orchard.ContentManagement.Records;
using System;

namespace MediaGarden.Models {
    public class MediaPartRecord : ContentPartRecord {
        public virtual String Url { get; set; }
        public virtual MediaSourceRecord Source { get; set; }
        public virtual String ViewerName{ get; set; }
        public virtual String FormatName{ get; set; }
    }
}
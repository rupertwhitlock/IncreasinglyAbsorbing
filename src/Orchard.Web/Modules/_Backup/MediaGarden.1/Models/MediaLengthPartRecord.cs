using Orchard;
using Orchard.ContentManagement.Records;
using System;

namespace MediaGarden.Models {
    public class MediaLengthPartRecord : ContentPartRecord {
        public virtual Decimal Length{ get; set; }
    }
}
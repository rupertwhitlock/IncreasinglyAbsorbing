using Orchard;
using Orchard.ContentManagement.Records;

namespace MediaGarden.Models {
    public class PixelDimensionsPartRecord : ContentPartRecord {
        public virtual int SizeX{ get; set; }
        public virtual int SizeY{ get; set; }
    }
}
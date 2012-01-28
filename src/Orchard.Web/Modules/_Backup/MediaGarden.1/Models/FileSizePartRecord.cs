using Orchard;
using Orchard.ContentManagement.Records;

namespace MediaGarden.Models {
    public class FileSizePartRecord : ContentPartRecord {
        public virtual long Size{ get; set; }

    }
}
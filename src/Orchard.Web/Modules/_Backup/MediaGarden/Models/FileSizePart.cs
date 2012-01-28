using Orchard;
using Orchard.ContentManagement;
namespace MediaGarden.Models {
    public class FileSizePart : ContentPart<FileSizePartRecord> {
        public long Size {
            get { return Record.Size; }
            set { Record.Size = value; }
        }

    }
}

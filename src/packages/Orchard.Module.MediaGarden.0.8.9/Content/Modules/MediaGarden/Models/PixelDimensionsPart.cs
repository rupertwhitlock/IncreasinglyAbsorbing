using Orchard;
using Orchard.ContentManagement;
using System.Drawing;

namespace MediaGarden.Models {
    /// <summary>
    /// X and Y dimensions of *pixel based* content
    /// TODO: Settings - units (px, cm, etc.). Was thinking I need to differentiate between source and final size; but perhaps not as that could be in the source record.
    /// Either way this should perhaps control *display* size whilst having a separate property for original size? ...
    /// </summary>
    public class PixelDimensionsPart : ContentPart<PixelDimensionsPartRecord> {
        public int SizeX {
            get { return Record.SizeX; }
            set { Record.SizeX = value; }
        }
        public int SizeY {
            get { return Record.SizeY; }
            set { Record.SizeY = value; }
        }

        public Size Size
        {
            get { return new Size(SizeX, SizeY); }
        }
    }
}

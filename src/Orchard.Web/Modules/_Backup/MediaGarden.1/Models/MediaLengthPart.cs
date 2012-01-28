using Orchard;
using Orchard.ContentManagement;
using System;

namespace MediaGarden.Models {
    /// <summary>
    /// Length of media in seconds
    /// </summary>
    public class MediaLengthPart : ContentPart<MediaLengthPartRecord> {
        public Decimal Length {
            get { return Record.Length; }
            set { Record.Length = value; }
        }

    }
}

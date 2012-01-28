using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace ThereAndBackAgain.WantedRecord.Models
{
    public class WantedRecordPart : ContentPart<WantedRecordPartRecord>
    {
        public string RecordArtists {
            get { return Record.RecordArtists;  }
            set { Record.RecordArtists = value; }
        }
    }
}

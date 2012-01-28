using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace ThereAndBackAgain.WantedRecord.Models
{
    public class WantedRecordPartRecord : ContentPartRecord
    {
        public virtual string RecordArtists { get; set; }
    }
}

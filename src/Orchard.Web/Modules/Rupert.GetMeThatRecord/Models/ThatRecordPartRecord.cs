using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace Rupert.GetMeThatRecord.Models
{
    public class ThatRecordPartRecord : ContentPartRecord
    {
        public ThatRecordPartRecord() {
            Tracks = new List<ThatRecordTrackRecord>();
        }

        public virtual string RecordUrl { get; set; }
        public virtual IList<ThatRecordTrackRecord> Tracks { get; set; }

    }
}

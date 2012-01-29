using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Rupert.GetMeThatRecord.Models
{
    public class ThatRecordPart : ContentPart<ThatRecordPartRecord>
    {
        [Required]
        public string RecordUrl {
            get { return Record.RecordUrl; }
            set { Record.RecordUrl = value; }
        }

        public IEnumerable<TrackRecord> Tracks {
            get { return Record.Tracks.Select(r => r.TrackRecord); }
        }
    }
}

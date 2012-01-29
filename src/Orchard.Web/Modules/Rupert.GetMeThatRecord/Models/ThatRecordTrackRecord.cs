using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rupert.GetMeThatRecord.Models
{
    public class ThatRecordTrackRecord
    {
        public virtual int Id { get; set; }
        public virtual ThatRecordPartRecord ThatRecordPartRecord { get; set; }
        public virtual TrackRecord TrackRecord { get; set; }
    }
}

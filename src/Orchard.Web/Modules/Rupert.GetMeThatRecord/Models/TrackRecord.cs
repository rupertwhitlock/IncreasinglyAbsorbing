using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace Rupert.GetMeThatRecord.Models
{
    public class TrackRecord
    {
        public virtual int Id { get; set; }
        public virtual string TrackUrl { get; set; }
    }
}

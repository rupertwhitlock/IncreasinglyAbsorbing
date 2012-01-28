using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Conventions;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;

namespace MediaGarden.Models
{
    /// <summary>
    /// Backing record for MediaSourceData
    /// </summary>
    public class MediaSourceRecord
    {
        public MediaSourceRecord()
        {
        }

        public virtual int Id { get; set; }
        public virtual String Location { get; set; }
        public virtual String FormatName { get; set; }
        public virtual String ViewerName { get; set; }
        public virtual String MediaStereotype { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual MediaSessionRecord SessionRecord { get; set; }

        [StringLengthMax]
        public virtual string Metadata { get; set; }

    }
}
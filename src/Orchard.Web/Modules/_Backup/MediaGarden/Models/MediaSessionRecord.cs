using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaGarden.Models
{
    public class MediaSessionRecord
    {
        public virtual int Id { get; set; }
        public virtual String Query { get; set; }
    }
}
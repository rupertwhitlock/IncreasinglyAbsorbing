using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace Contrib.HttpMp3Track.Models
{
    public class HttpMp3TrackPartRecord : ContentPartRecord
    {
        public virtual string Url { get; set; }
    }
}

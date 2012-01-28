using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Contrib.HttpMp3Track.Models
{
    public class HttpMp3TrackPart : ContentPart<HttpMp3TrackPartRecord>
    {
        
        public string Url {
            get { return Record.Url; }
            set { Record.Url = value; }
        }
    }
}

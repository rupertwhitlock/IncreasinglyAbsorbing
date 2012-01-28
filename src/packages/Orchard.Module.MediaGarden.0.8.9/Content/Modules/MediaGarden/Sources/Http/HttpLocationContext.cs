using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources.Http
{
    public class HttpHeaderContext : MediaHeaderContext
    {
        public HttpHeaderContext(MediaLocationContext location)
            : base(location)
        {
        }

        public System.Net.WebHeaderCollection Headers { get; set; }

        public string ContentTypeParameter { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Pipeline;
using System.Net;

namespace MediaGarden.Sources.Http
{
    public class HttpStreamAccessor : IStreamAccessor
    {
        private string url;

        public HttpStreamAccessor(string url)
        {
            // TODO: Complete member initialization
            this.url = url;
        }

        public System.IO.Stream GetStream()
        {
            WebRequest WebRequestObject = HttpWebRequest.Create(url);
            WebResponse ResponseObject = WebRequestObject.GetResponse();
            return ResponseObject.GetResponseStream();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources.Http
{
    /// <summary>
    /// </summary>
    public class HttpLocationFilter : IMediaLocationFilter {
    

        public string LocationName
        {
            get { return "Http"; }
        }

        public void LocationFiltering(MediaQueryContext query, MediaLocationContext location)
        {
            if (location.QueryName != "Http") return;

            query.Headers.Add(ProbeHttpHeaders(location));
        }

        public void LocationFiltered(MediaQueryContext query, MediaLocationContext location)
        {
            return;
        }

        public static HttpHeaderContext ProbeHttpHeaders(MediaLocationContext location)
        {
            var url = location.Location;
            WebRequest WebRequestObject = HttpWebRequest.Create(url);
            // Begin GET
            WebRequestObject.Method = "GET";
            WebResponse ResponseObject = WebRequestObject.GetResponse();

            // Extract title from URL
            // TODO: Could be an extension method
            var title = url.ExtractFileName();

            var headers = new HttpHeaderContext(location)
            {
                ContentLength = ResponseObject.ContentLength,
                Headers = ResponseObject.Headers,
                HashCode = ResponseObject.GetHashCode(),
                Title = title
            };
            string contentType = ResponseObject.ContentType;
            string contentTypeParameter = "";
            int index = contentType.IndexOf(";");
            if (index > 0)
            {
                contentType = contentType.Substring(0, index);
                // e.g. CHARSET
                contentTypeParameter = contentType.Substring(index+1).Trim();
            }
            headers.ContentType = contentType;
            headers.ContentTypeParameter = contentTypeParameter;
            if (ResponseObject.Headers["Last-Modified"]!=null) {
                DateTime timeStamp;
                if (!DateTime.TryParse(ResponseObject.Headers["Last-Modified"],out timeStamp))
                {
                    timeStamp = DateTime.Now;
                }
                headers.TimeStamp = timeStamp;
            }
            ResponseObject.Close();
            return headers;
        }

    }
}

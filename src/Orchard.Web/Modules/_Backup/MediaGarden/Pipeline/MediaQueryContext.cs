using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    public class MediaQueryContext
    {

        /// <summary>
        /// Location of search; for instance, media folder path or feed URL
        /// </summary>
        public String Query { get; set; }

        /// <summary>
        /// Any other settings required by a particular source
        /// </summary>
        public dynamic Settings { get; set; }

        public List<MediaLocationContext> Locations { get; protected set; }
        public List<MediaHeaderContext> Headers { get; protected set; }
        public List<MediaSourceContext> Sources { get; protected set; }

        public MediaQueryContext()
        {
            Locations = new List<MediaLocationContext>();
            Headers = new List<MediaHeaderContext>();
            Sources = new List<MediaSourceContext>();
        }

        public string MediaStereotype { get; set; }
    }
}

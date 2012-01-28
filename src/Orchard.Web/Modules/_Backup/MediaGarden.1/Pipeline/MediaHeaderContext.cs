using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaGarden.Pipeline
{
    public class MediaHeaderContext
    {
        public MediaHeaderContext(MediaLocationContext location)
        {
            LocationContext = location;
        }

        public MediaLocationContext LocationContext { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Title { get; set; }
        public int HashCode { get; set; }
    }
}

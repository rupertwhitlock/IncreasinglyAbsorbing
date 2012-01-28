using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaGarden.Pipeline
{
    public class MediaLocationContext
    {
        public string QueryName { get; set; }
        public string Location { get; set; }
        public IStreamAccessor Stream { get; set; }
    }
}

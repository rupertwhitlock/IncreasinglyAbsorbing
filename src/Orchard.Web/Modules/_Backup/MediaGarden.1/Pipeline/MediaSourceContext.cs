using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// Holds information about media source including data accessors
    /// </summary>
    public class MediaSourceContext
    {

        private readonly IMediaSource _source;
        
        public IMediaSource Source { get { return _source; } }
        public IStreamAccessor Stream { get; set; }

        public MediaSourceContext(IMediaSource source)
        {
            _source = source;
        }

        public IEnumerable<IMediaFormat> Formats { get; set; }
    }
}
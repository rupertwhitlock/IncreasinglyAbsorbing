using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// Context for containing media item while building
    /// </summary>
    public class MediaItemContext
    {

        public IMediaItem Item { get; set; }

    }
}
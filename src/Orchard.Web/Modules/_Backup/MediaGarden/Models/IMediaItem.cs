using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Formats;
using MediaGarden.Pipeline;
using Orchard.ContentManagement;

namespace MediaGarden.Models
{
    public interface IMediaItem
    {
        String Title { get; }
        String MediaUrl { get; }
        string MediaStereotype { get; }
        IMediaFormat MediaFormat { get; }
        /// <summary>
        /// TODO: Fairly redundant (at least on this interface)
        /// </summary>
        IMediaSource MediaSource { get; }
        IMediaViewer MediaViewer { get; }
    }
}

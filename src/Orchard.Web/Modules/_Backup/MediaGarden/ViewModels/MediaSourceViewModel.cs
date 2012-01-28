using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;
using MediaGarden.Pipeline;

namespace MediaGarden.ViewModels
{
    /// <summary>
    /// Wrapper model for generating media source shapes
    /// TODO: Could do with some kind of dynamic property for additional model data
    /// </summary>
    public class MediaSourceViewModel
    {

        public MediaSourceViewModel(IMediaSource source)
        {
            Source = source;
            Selected = false;
        }

        public IMediaSource Source { get; protected set; }
        public bool Selected { get; set; }
        public IMediaItem Preview { get; set; }
        /// <summary>
        /// Name of content type to create from this source
        /// </summary>
        public string ContentTypeName { get; set; }
    }
}
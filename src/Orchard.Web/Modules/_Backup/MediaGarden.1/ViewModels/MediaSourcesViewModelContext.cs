using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Pipeline;
using MediaGarden.ViewModels;

namespace MediaGarden.ViewModels
{
    public class MediaSourcesViewModelContext
    {
        /// <summary>
        /// The Sources UI shape
        /// </summary>
        public dynamic Shape { get; set; }

        /// <summary>
        /// The resultant model
        /// </summary>
        public MediaSourcesViewModel Model { get; set; }

    }
}

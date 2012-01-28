using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;

namespace MediaGarden.ViewModels
{
    /// <summary>
    /// This model wraps the whole shape building process for the media sources page
    /// </summary>
    public class MediaSourcesViewModel
    {
        public MediaSourcesViewModel()
        {
            Inputs = new MediaInputsViewModel();
        }

        public MediaInputsViewModel Inputs { get; set; }

        public int? MediaSessionId { get; set; }

        public string MediaStereotype { get; set; }

        public IEnumerable<MediaSourceViewModel> Sources { get; set; }
    }
}
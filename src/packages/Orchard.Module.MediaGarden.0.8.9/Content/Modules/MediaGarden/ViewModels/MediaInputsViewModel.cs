using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Downplay.Origami;
using Downplay.Origami.Services;
using MediaGarden.Pipeline;

namespace MediaGarden.ViewModels
{
    /// <summary>
    /// This module is used to generate the inputs (and for them to populate on postback)
    /// </summary>
    public class MediaInputsViewModel
    {
        public List<String> Queries { get; protected set; }
        public List<MediaHeaderContext> Headers { get; protected set; }
        public string Fetch { get; set; }
        public MediaInputsViewModel() {
            Queries = new List<String>();
            Headers = new List<MediaHeaderContext>();
        }
    }
}

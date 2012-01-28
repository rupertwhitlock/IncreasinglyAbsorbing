using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaGarden.Sources.ViewModels
{
    public class SourceFormatViewModel
    {

        public string Stereotype { get; set; }
        public IEnumerable<SelectListItem> Stereotypes { get; set; }

        public string ContentTypeName { get; set; }
        public IEnumerable<SelectListItem> ContentTypes { get; set; }

        public string FormatName { get; set; }
        public IEnumerable<SelectListItem> Formats { get; set; }

        public string ViewerName { get; set; }
        public IEnumerable<SelectListItem> Viewers { get; set; }


    }
}
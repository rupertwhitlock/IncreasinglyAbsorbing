using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.jPlayer.ViewModels;

namespace Orchard.jPlayer.Models {
    public class MediaGalleryPart : ContentPart<MediaGalleryRecord> {
        public virtual string MediaGalleryName {
            get { return Record.MediaGalleryName; }
            set { Record.MediaGalleryName = value; }
        }

        public bool HasAvailableGalleries {
            get { return AvailableGalleries != null && AvailableGalleries.Count() > 0; }
        }

        public string SelectedGallery { get; set; } // used on editor

        public IEnumerable<SelectListItem> AvailableGalleries { get; set; } // used on editor

        public Plugins.MediaType SelectedType {
            get { return (Plugins.MediaType)Record.MediaType; }
            set { Record.MediaType = (byte)value; }
        }

        public bool AutoPlay {
            get { return Record.AutoPlay; }
            set { Record.AutoPlay = value; }
        }

        public IEnumerable<SelectListItem> AvailableTypes { get; set; } // used on editor

        public virtual MediaGalleryViewModel ViewModel { get; set; } // used on display
    }
}
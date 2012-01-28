using Orchard;
using Orchard.ContentManagement;
using System;
using MediaGarden.Pipeline;
using Orchard.Core.Routable.Models;
using System.Linq;

namespace MediaGarden.Models {
    public class MediaPart : ContentPart<MediaPartRecord>, IMediaItem {

        public String Url {
            get { return Record.Url; }
            set { Record.Url = value; }
        }
        public MediaSourceRecord Source {
            get { return Record.Source; }
            set { Record.Source = value; }
        }
        public String ViewerName {
            get { return Record.ViewerName; }
            set { Record.ViewerName = value; }
        }
        public String FormatName {
            get { return Record.FormatName; }
            set { Record.FormatName = value; }
        }

        public string MediaUrl
        {
            get {
                if (Source == null) return "";
                return Source.Location; }
        }

        public IMediaFormat MediaFormat { get; set; }

        public IMediaSource MediaSource { get; set; }
    
        public IMediaViewer MediaViewer { get; set; }

        public string Title
        {
            get
            {
                // Use dynamic to avoid dependencies
                dynamic content = this.ContentItem;

                // Following checks are really messy due to no null check available on dynamic content item parts

                // Check for ITitledAspect (Mechanics.Plumbing)
                if (ContentItem.Parts.Any(p => p.PartDefinition.Name == "TitlePart"))
                {
                    return content.TitlePart.Title;
                }
                // Check for RoutePart
                if (ContentItem.Parts.Any(p => p.PartDefinition.Name == "RoutePart"))
                {
                    return content.RoutePart.Title;
                }
                return Url;
            }
        }

        public string MediaStereotype
        {
            get { return TypeDefinition.Settings["MediaStereotype"]; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MediaGarden.Models;
using MediaGarden.Pipeline;

using Orchard.Environment.Extensions;

namespace MediaGarden.Defaults
{
    [OrchardFeature("MediaGarden.Defaults")]
    public class DownloadLinkViewer : IMediaViewer
    {
        public string ViewerName
        {
            get { return "DownloadLink"; }
        }

        public string ViewerDescription
        {
            get { return "Download link"; }
        }

        public IEnumerable<string> SupportedMediaFormats()
        {
            // For now, empty collection means all
            // TODO: This shouldn't handle certain formats; ATM only embed and RTMP that I can think of. So maybe an UnsupportedMediaFormats is more appropriate here.
            yield break;
        }

        public int ViewerPriority(MediaViewerContext context)
        {
            // This is the fallback so always use a zero priority
            return 0;
        }
    }
}
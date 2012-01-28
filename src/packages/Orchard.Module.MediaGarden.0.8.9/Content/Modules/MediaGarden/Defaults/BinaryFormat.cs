using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Services;
using Orchard.Environment.Extensions;
using MediaGarden.Models;
using MediaGarden.Pipeline;

namespace MediaGarden.Defaults
{
    [OrchardFeature("MediaGarden.Defaults")]
    public class BinaryFormat : IMediaFormat
    {
        public string FormatName
        {
            get { return "Binary"; }
        }

        public string FormatDisplayName
        {
            get
            {
                return "Binary file";
            }
        }

        public string MediaStereotype
        {
            get { return "Binary"; }
        }

        public bool IsOfFormat(MediaSourceContext source)
        {
            // All formats count as binary
            // TODO: Could do basic check to eliminate text formats. Maybe change implementation of MimeMediaFormat slightly so we can do != checks as well as ==
            return true;
        }
    }
}
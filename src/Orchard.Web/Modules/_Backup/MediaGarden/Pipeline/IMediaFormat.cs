using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    public interface IMediaFormat : IDependency
    {
        String FormatName { get; }
        String FormatDisplayName { get; }
        String MediaStereotype { get; }
        bool IsOfFormat(MediaSourceContext source);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    public interface IMediaSourceFilter : IDependency
    {
        IEnumerable<String> SupportedFormats();
        void BuildSourceMetadata(MediaQueryContext query, MediaLocationContext location, MediaSourceContext source);
        void SourceFiltering(MediaContentContext contentContext, IMediaSource source);
        void SourceFiltered(MediaContentContext contentContext, IMediaSource source);
    }
}

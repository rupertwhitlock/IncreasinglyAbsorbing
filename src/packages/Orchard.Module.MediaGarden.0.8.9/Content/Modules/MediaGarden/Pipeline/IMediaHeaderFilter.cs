using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;
using Orchard;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// TODO: The main thing we're lacking here is a way to eliminate items that have already been processed.
    /// So when new files are uploaded or a feed is checked we're not re-importing items we already have in the DB.
    /// </summary>
    public interface IMediaHeaderFilter : IDependency
    {
        String HeaderName { get; }
        void HeaderFiltering(MediaQueryContext query, MediaHeaderContext header);
        void HeaderFiltered(MediaQueryContext query, MediaHeaderContext header);
    }
}

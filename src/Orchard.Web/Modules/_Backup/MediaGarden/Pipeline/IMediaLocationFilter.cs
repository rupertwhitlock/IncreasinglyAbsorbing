using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace MediaGarden.Pipeline
{
    public interface IMediaLocationFilter : IDependency
    {
        String LocationName { get; }
        void LocationFiltering(MediaQueryContext query, MediaLocationContext location);
        void LocationFiltered(MediaQueryContext query, MediaLocationContext location);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// Filters the inputted location to determine in which way we need to access it
    /// </summary>
    public interface IMediaQueryFilter : IDependency
    {
        void QueryFiltering(MediaQueryContext context);
        void QueryFiltered(MediaQueryContext context);
    }
}
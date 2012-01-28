using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaGarden.Variations
{
    public interface IVariant
    {

        /// <summary>
        /// Part type on which to track the variation
        /// </summary>
        /// <returns></returns>
        Type TypeVaried();

        /// <summary>
        /// Other part types that will additionally have variants
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> AlsoVaried();

    }
}
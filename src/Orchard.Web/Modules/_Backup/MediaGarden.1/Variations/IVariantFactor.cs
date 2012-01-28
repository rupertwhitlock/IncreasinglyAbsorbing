using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaGarden.Variations
{
    public interface IVariantFactor<T>
    {
        Func<T, object> FactorAccessor { get; }
    }
}

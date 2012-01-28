using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaGarden.Variations
{
    public interface IVariantsProvider
    {

        IEnumerable<IVariant> GetVariants();

    }
}
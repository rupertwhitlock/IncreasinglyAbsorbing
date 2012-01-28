using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace MediaGarden.Variations
{
    public class VariantFactor<T> : IVariantFactor<T>
        where T:IContent
    {
        private Func<T, object> f;

        public VariantFactor(Func<T, object> f)
        {
            // TODO: Complete member initialization
            this.f = f;
        }

        public Func<T, object> FactorAccessor
        {
            get { return f; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace MediaGarden.Variations
{
    public class Variant<T> : IVariant
        where T:IContent
    {

        public string Name { get; set; }
        public IEnumerable<IVariantFactor<T>> UniqueFactors { get; set; }

        public Variant(string name, params Func<T, object>[] factors)
        {
            Name = name;
            UniqueFactors = factors.Select(f=>new VariantFactor<T>(f));
            _alsoVaried =new List<Type>();
        }

        public Type TypeVaried()
        {
            return typeof(T);
        }

        protected List<Type> _alsoVaried;

        public IEnumerable<Type> AlsoVaried()
        {
            return _alsoVaried;
        }

        /// <summary>
        /// Fluent setter for adding AlsoVaried types
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Variant<T> AlsoVaried<TVary>()
        {
            _alsoVaried.Add(typeof(TVary));
            return this;
        }
    }
}
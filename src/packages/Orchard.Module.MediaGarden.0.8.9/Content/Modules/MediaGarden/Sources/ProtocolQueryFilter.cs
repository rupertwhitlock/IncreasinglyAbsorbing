using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources
{
    /// <summary>
    /// Base implementation for a protocol-based query filter
    /// </summary>
    public abstract class ProtocolQueryFilter : IMediaQueryFilter
    {

        protected abstract IEnumerable<String> Protocols();

        public void QueryFiltering(MediaQueryContext context)
        {
            var protocol = GetProtocol(context.Query);
            if (String.IsNullOrWhiteSpace(protocol)) return;
            if (Protocols().Any(p => p == protocol))
            {
                OnQueryFiltering(context, protocol);
            }
        }

        /// <summary>
        /// TODO: Code repeated from QueryFiltering - could optimise slightly
        /// </summary>
        /// <param name="context"></param>
        public void QueryFiltered(MediaQueryContext context)
        {
            var protocol = GetProtocol(context.Query);
            if (String.IsNullOrWhiteSpace(protocol)) return;
            if (Protocols().Any(p => p == protocol))
            {
                OnQueryFiltered(context, protocol);
            }
        }

        
        /// <summary>
        /// Extract protocol string from location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        protected string GetProtocol(string location)
        {
            var pos = location.LastIndexOf(':');
            if (pos < 0) return null;
            return location.Substring(0, pos);
        }

        protected abstract void OnQueryFiltering(MediaQueryContext context, string protocol);
        protected abstract void OnQueryFiltered(MediaQueryContext context, string protocol);

    }
}
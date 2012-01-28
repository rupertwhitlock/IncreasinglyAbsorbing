using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources.Http
{
    public class HttpQueryFilter : ProtocolQueryFilter
    {
        protected override IEnumerable<string> Protocols()
        {
            yield return "http";
            yield return "https";
        }

        protected override void OnQueryFiltering(MediaQueryContext context, string protocol)
        {
            // TODO: We *could* do more complex parsing to automatically determine mysite.com/filename pattern without http but I don't reckon it's worth it.
            context.Locations.Add(new MediaLocationContext(){
                Location = context.Query,
                QueryName = "Http",
                Stream = new HttpStreamAccessor(context.Query)
            });
        }

        protected override void OnQueryFiltered(MediaQueryContext context, string protocol)
        {
            // TODO: Cleanup, ensure HTTP handles are closed (later)
        }
    }
}
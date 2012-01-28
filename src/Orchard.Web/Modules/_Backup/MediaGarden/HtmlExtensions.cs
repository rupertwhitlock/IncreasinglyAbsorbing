using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Utility.Extensions;
using System.Text.RegularExpressions;

namespace MediaGarden
{
    public static class HtmlExtensions
    {

        /// <summary>
        /// Concerts anonymous object into FlashVars string (like querystring but HTML encoded)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IHtmlString AnonymousToFlashVars(this HtmlHelper html, UrlHelper url, object values) {
            var dict = new RouteValueDictionary(values);
            // HTML and URL encode, glue and convert
            return MvcHtmlString.Create(
                dict.Select(kv => 
                    html.Encode(url.Encode(kv.Key)) + "=" + html.Encode(url.Encode(kv.Value.ToString())))
                    .Glue("&amp;"));
        }
        public static string HtmlClassify(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            var friendlier = text.CamelFriendly();
            return Regex.Replace(friendlier, @"[^a-zA-Z0-9]+", m => m.Index == 0 ? "" : "-").ToLowerInvariant();
        }

    }
}
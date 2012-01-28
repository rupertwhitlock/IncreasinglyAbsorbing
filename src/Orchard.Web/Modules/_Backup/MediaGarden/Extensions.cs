using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Routable.Models;

namespace MediaGarden
{
    public static class Extensions
    {

        /// <summary>
        /// Dynamically checking a part exists and performing an action on it
        /// </summary>
        /// <param name="content"></param>
        /// <param name="partName"></param>
        /// <param name="action"></param>
        public static void If(this IContent content, string partName, Action<dynamic> action)
        {
            if (content.ContentItem.Parts.Any(p => p.PartDefinition.Name == partName))
            {
                action.Invoke(content);
            }
        }

        public static void With<T>(this IContent content, Action<T> action) where T : IContent
        {
            T with = content.As<T>();
            if (with != null)
            {
                action.Invoke(with);
            }
        }

        public static void WithPart<T>(this IEnumerable<ContentItem> items, Action<T> action) where T : IContent
        {
            foreach (var content in items.AsPart<T>())
            {
                action.Invoke(content);
            }
        }

        public static string GetTitle(this IContent target)
        {
            // Use Id or Routable title
            // TODO: Localization of title here perhaps
            var title = target.ContentItem.ContentType + " " + target.Id.ToString();
            dynamic content = target.ContentItem;
            if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "TitlePart")) // content.UserPart != null)
            {
                title = content.TitlePart.Title;
            }

            // Site name?
            // ... Hmm ...

            // Following checks are really messy due to no null check available on dynamic content item parts

            // User name?
            if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "UserPart")) // content.UserPart != null)
            {
                title = content.UserPart.UserName;
            }

            // Route title?
            if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "RoutePart")) //(content.RoutePart != null)
            {
                title = content.RoutePart.Title;
            }

            // Widget title?
            if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "WidgetPart")) //(content.RoutePart != null)
            {
                title = content.WidgetPart.Title;
            }

            return title;
        }

        public static string ExtractFileName(this string pathAndFileName)
        {
            var title = pathAndFileName.Trim('/');
            var lastDot = title.LastIndexOf('.');
            if (lastDot > 0) title = title.Substring(0, lastDot);
            var lastSlash = title.LastIndexOf('/');
            if (lastSlash > 0) title = title.Substring(lastSlash + 1);
            return title;
        }


        /// <summary>
        /// Parse string to an Int32 value. Returns null if unable to parse.
        /// Slightly friendlier than Int32.Parse or even Int32.TryParse!
        /// </summary>
        /// <seealso cref="ParseLong"/>
        /// <param name="s">The input string to parse</param>
        /// <returns>Null if parse fails, others nullable int of parsed value</returns>
        public static Int32? ParseInt(this String s)
        {
            int result;
            if (Int32.TryParse(s, out result))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// Parse string to an Int64 value. Returns null if unable to parse.
        /// Slightly friendlier than Int64.Parse and especially more than Int64.TryParse!
        /// </summary>
        /// <seealso cref="ParseInt"/>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Int64? ParseLong(this String s)
        {
            Int64 result;
            if (Int64.TryParse(s, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Glues a number of string parts together with a separator
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string Glue(this IEnumerable<String> parts, string glue)
        {
            return String.Join(glue, parts.ToArray());
        }

        /// <summary>
        /// Checks for a k/v pair in a dictionary avoiding null errors etc.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has(this IDictionary<String, String> dict, string key, string value)
        {
            return dict.ContainsKey(key) && dict[key] == value;
        }

        public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return defaultValue;
        }

    }
}
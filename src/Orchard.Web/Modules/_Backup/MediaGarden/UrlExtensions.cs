using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Utility.Extensions;
using Orchard.Mvc.Html;
using MediaGarden.Models;
namespace MediaGarden
{
    public static class UrlExtensions
    {
        public static string AbsoluteDisplayUrl(this UrlHelper url, IContent item)
        {
            var siteUrl = url.RequestContext.HttpContext.Request.ToRootUrlString();
            return siteUrl+url.ItemDisplayUrl(item);
        }
        public static string Absolute(this UrlHelper url, string contentPath)
        {
            var siteUrl = url.RequestContext.HttpContext.Request.ToRootUrlString();
            return siteUrl + url.Content(contentPath);
        }

    }
}
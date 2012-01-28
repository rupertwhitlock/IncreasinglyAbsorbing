using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MediaGarden
{
    /// <summary>
    /// Extensions to help when we haven't got access to object instances like UrlHelper or HtmlHelper
    /// </summary>
    public static class Null
    {

        public static UrlHelper Url
        {
            get
            {
                return new UrlHelper(new RequestContext(
                    new HttpContextWrapper(HttpContext.Current),
                    new RouteData()), RouteTable.Routes);
            }
        }


    }
}
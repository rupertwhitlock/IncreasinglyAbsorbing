using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Models;

namespace MediaGarden.Pipeline
{
    public static class MediaSourceExtensions
    {

        public static string FileExtension(this IMediaSource item)
        {
            var dot = item.Url.LastIndexOf('.');
            if (dot < 0) return "";
            return item.Url.Substring(dot + 1);
        }

    }
}
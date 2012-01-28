using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using System.IO;
using Microsoft.Win32;

namespace MediaGarden.Sources.FileSystem
{
    /// <summary>
    /// TODO: Not sure if we need both HeaderFilter and LocationFilter for file systems. Could just skip this step.
    /// </summary>
    public class FileSystemLocationFilter : IMediaLocationFilter
    {
        public string LocationName
        {
            get { return "FileSystem"; }
        }

        public void LocationFiltering(MediaQueryContext query, MediaLocationContext location)
        {
            // TODO: Could be an interface instead for further extensibility
            if (!(location is FileSystemLocationContext)) return;

            var fileLocation = location as FileSystemLocationContext;
            // Add header detail
            query.Headers.Add(
                new MediaHeaderContext(location)
                {
                    ContentLength = fileLocation.File.GetSize(),
                    // TODO: MIME will be auto-accepted since we leave ContentType blank; so format match will just be on extension. Not a viable long-term solution.
                    ContentType = "", //fileLocation.File.GetFileType(),// .Trim('.').ToLower()),
                    HashCode = fileLocation.File.GetHashCode(),
                    TimeStamp = fileLocation.File.GetLastUpdated(),
                    Title = fileLocation.File.GetName().ExtractFileName()
                });
        }

        public void LocationFiltered(MediaQueryContext query, MediaLocationContext location)
        {
            if (!(location is FileSystemLocationContext)) return;
        }

        /*
        /// <summary>
        /// *Surely* there would be a trust issue with this...
        /// TODO: Yes there was :)
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        static string GetMimeType(string extension)
        {
            var mimeType = "application/unknown";
            var regKey = Registry.ClassesRoot.OpenSubKey(extension);
            if (regKey != null)
            {
                var contentType = regKey.GetValue("Content Type");
                if (contentType != null) mimeType = contentType.ToString();
            }
            return mimeType;
        }*/
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Orchard.jPlayer.Models.Plugins {
    public abstract class MediaGalleryType {
        protected virtual MediaType MediaType {
            get { throw new NotImplementedException("The plugin has not implemented the MediaType"); }
        }

        public virtual string MediaGalleryTemplateName {
            get { throw new NotImplementedException("The plugin has not implemented the Media Gallery Template"); }
        }

        public virtual MvcHtmlString Javascript(IEnumerable<MediaGalleryMedia> medias, bool autoPlay, string swfPath, string id) {
            var sb = new StringBuilder();

            var playlist = new List<string>();
            var supplied = new List<string>();
            foreach(var media in medias.OrderBy(x => x.Position)) {
                var extension = GetExtensionType(media, MediaType);
                playlist.Add("{ " + CreateItem(media, extension) + " }");
                if(!supplied.Contains(extension))
                    supplied.Add(extension);
            }

            sb.AppendFormat("var audioPlaylist = new Playlist(\"{0}\", [ {1} ],", id, string.Join(", ", playlist.ToArray()));
            sb.AppendLine(" {");
#if(DEBUG)
            sb.AppendLine("     errorAlerts: true,");
#endif
            sb.AppendFormat("       swfPath: \"{0}\",\n", swfPath);
            sb.AppendFormat("       supplied: \"{0}\",\n", string.Join("\", \"", supplied.ToArray()));
            sb.AppendLine("         ready: function() {");
            sb.AppendLine("             audioPlaylist.displayPlaylist();");
            sb.AppendFormat("           audioPlaylist.playlistInit({0})\n", autoPlay ? "true" : "false");
            sb.AppendLine("         },");
            sb.AppendLine("         ended: function() {");
            sb.AppendLine("             audioPlaylist.playlistNext();");
            sb.AppendLine("         },");
            sb.AppendLine("         play: function() {");
            sb.AppendLine("             $(this).jPlayer(\"pauseOthers\");");
            sb.AppendLine("         }");
            sb.AppendLine("     });");

            return MvcHtmlString.Create(sb.ToString());
        }

        protected string CreateItem(MediaGalleryMedia media, string extension) {
            var title = media.Title;
            if(string.IsNullOrEmpty(title))
                title = media.Name.Substring(0, media.Name.LastIndexOf('.'));

            return string.Format("name: \"{0}\", {1}: \"{2}\"", title, extension, media.PublicUrl);
        }

        protected string GetExtensionType(MediaGalleryMedia media, MediaType mediaType) {
            var extension = media.Name.Substring(media.Name.LastIndexOf('.') + 1);

            switch(extension) {
                case "ogg":
                    return mediaType == MediaType.Video ? "ogv" : "oga";
                case "webm":
                    return mediaType == MediaType.Video ? "webmv" : "webma";
                default:
                    return extension;
            }
        }
    }
}
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Orchard.jPlayer.Models.Plugins.Audio {
    public class Audio : MediaGalleryType {
        public override string MediaGalleryTemplateName {
            get { return "Parts/Audio"; }
        }

        protected override MediaType MediaType {
            get {
                return MediaType.Audio;
            }
        }

        public override MvcHtmlString Javascript(IEnumerable<MediaGalleryMedia> medias, bool autoPlay, string swfPath, string id) {
            var sb = new StringBuilder();
            sb.AppendLine(base.Javascript(medias, autoPlay, swfPath, id).ToString());

            // Exists an issue when using flash, the ready function is not called.
            // This issue appears to be only in Audio mode.
            // I need to try to initialize again
            // The functions have an flag to not execute more then once
            sb.AppendLine("if(audioPlaylist != undefined) {");
            sb.AppendLine("     audioPlaylist.displayPlaylist();");
            sb.AppendFormat("   audioPlaylist.playlistInit({0})\n", autoPlay ? "true" : "false");
            sb.AppendLine("}");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
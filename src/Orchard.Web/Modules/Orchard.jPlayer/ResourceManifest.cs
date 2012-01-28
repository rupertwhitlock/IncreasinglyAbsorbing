using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace Orchard.jPlayer {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            builder.Add().DefineStyle("MediaGalleryAdmin").SetUrl("media-gallery-admin.css");
            builder.Add().DefineStyle("MediaPlayer").SetUrl("jplayer.blue.monday.css");

            builder.Add().DefineScript("jQueryMultiFile").SetDependencies("jquery").SetUrl("jquery.MultiFile.pack.js");
            builder.Add().DefineScript("jQueryUISortable").SetDependencies("jquery").SetUrl("sortable-interaction-jquery-ui-1.8.10.custom.min.js");
            builder.Add().DefineScript("jQueryJSON").SetDependencies("jquery").SetUrl("jquery.json-2.2.min.js");
            builder.Add().DefineScript("jQueryPlayer").SetDependencies("jquery").SetUrl("jquery.jplayer.min.js", "jquery.jplayer.js");
            builder.Add().DefineScript("jPlayerPlaylist").SetDependencies("jQueryPlayer").SetUrl("playlist.js");
        }
    }
}
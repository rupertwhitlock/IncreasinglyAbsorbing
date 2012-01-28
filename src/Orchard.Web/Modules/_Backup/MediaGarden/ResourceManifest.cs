using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace MediaGarden
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add().DefineScript("jQueryMultiFile").SetDependencies("jquery").SetUrl("jquery.MultiFile.pack.js");
        }
    }
}
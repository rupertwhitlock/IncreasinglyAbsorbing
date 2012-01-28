using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.jPlayer.Utils;
using Orchard.UI.Resources;

namespace Orchard.jPlayer.Models.Plugins {
    public abstract class TypeResourceDescriptor {
        private const string TypeResourceBasePath = "~/Modules/Orchard.jPlayer/Content/Plugins/";

        public string TypeResourcePath {
            get { return VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(TypeResourceBasePath), TypeName); }
        }

        public abstract string TypeName { get; }

        private IList<string> _scripts = new List<string>();

        public IList<string> Scripts {
            get { return _scripts; }
            set { _scripts = value; }
        }

        private IList<LinkEntry> _styles = new List<LinkEntry>();

        public IList<LinkEntry> Styles {
            get { return _styles; }
            set { _styles = value; }
        }

        public void AddScript(string scriptPath) {
            Scripts.Add(JavaScriptHelper.AddScriptTag(string.Concat(TypeResourcePath, "/", scriptPath)));
        }

        public void AddStyle(string styleSheetPath) {
            Styles.Add(LinkHelper.BuildStyleLink(string.Concat(TypeResourcePath, "/", styleSheetPath)));
        }
    }
}
﻿using System;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Xml.Linq;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.WebSite;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI;
using Orchard.UI.Admin;

namespace Orchard.DesignerTools.Services {
    [OrchardFeature("Orchard.DesignerTools")]
    public class ShapeTracingFactory : IShapeFactoryEvents, IShapeDisplayEvents {
        private readonly WorkContext _workContext;
        private readonly IShapeTableManager _shapeTableManager;
        private readonly IThemeManager _themeManager;
        private readonly IWebSiteFolder _webSiteFolder;
        private readonly IAuthorizer _authorizer;
        private int _shapeId;

        public ShapeTracingFactory(
            WorkContext workContext, 
            IShapeTableManager shapeTableManager, 
            IThemeManager themeManager, 
            IWebSiteFolder webSiteFolder,
            IAuthorizer authorizer
            ) {
            _workContext = workContext;
            _shapeTableManager = shapeTableManager;
            _themeManager = themeManager;
            _webSiteFolder = webSiteFolder;
            _authorizer = authorizer;
        }

        private bool IsActivable() {
            // activate on front-end only
            if (AdminFilter.IsApplied(new RequestContext(_workContext.HttpContext, new RouteData())))
                return false;

            // if not logged as a site owner, still activate if it's a local request (development machine)
            if (!_authorizer.Authorize(StandardPermissions.SiteOwner))
                return _workContext.HttpContext.Request.IsLocal;

            return true;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if(!IsActivable()) {
                return;
            }

            if (context.ShapeType != "Layout"
                && context.ShapeType != "DocumentZone"
                && context.ShapeType != "PlaceChildContent"
                && context.ShapeType != "ContentZone"
                && context.ShapeType != "ShapeTracingMeta"
                && context.ShapeType != "ShapeTracingTemplates"
                && context.ShapeType != "DateTimeRelative") {

                var shapeMetadata = (ShapeMetadata)context.Shape.Metadata;
                var currentTheme = _themeManager.GetRequestTheme(_workContext.HttpContext.Request.RequestContext);
                var shapeTable = _shapeTableManager.GetShapeTable(currentTheme.Id);

                if (!shapeTable.Descriptors.ContainsKey(shapeMetadata.Type)) {
                    return;
                }

                shapeMetadata.Wrappers.Add("ShapeTracingWrapper");
                shapeMetadata.OnDisplaying(OnDisplaying);
            }
        }
        public void Displaying(ShapeDisplayingContext context) {}

        public void OnDisplaying(ShapeDisplayingContext context) {
            if (!IsActivable()) {
                return;
            }

            var shape = context.Shape;
            var shapeMetadata = (ShapeMetadata) context.Shape.Metadata;
            var currentTheme = _themeManager.GetRequestTheme(_workContext.HttpContext.Request.RequestContext);
            var shapeTable = _shapeTableManager.GetShapeTable(currentTheme.Id);

            if (!shapeMetadata.Wrappers.Contains("ShapeTracingWrapper")) {
                return;
            }

            var descriptor = shapeTable.Descriptors[shapeMetadata.Type];

            // dump the Shape's content
            var dump = new ObjectDumper(6).Dump(context.Shape, "Model");

            var sb = new StringBuilder();
            ConvertToJSon(dump, sb);
            shape._Dump = sb.ToString();

            shape.Template = null;
            shape.OriginalTemplate = descriptor.BindingSource;

            foreach (var extension in new[] { ".cshtml", ".aspx" }) {
                foreach (var alternate in shapeMetadata.Alternates.Reverse().Concat(new [] {shapeMetadata.Type}) ) {
                    var alternateFilename = FormatShapeFilename(alternate, shapeMetadata.Type, shapeMetadata.DisplayType, currentTheme.Location + "/" + currentTheme.Id, extension);
                    if (_webSiteFolder.FileExists(alternateFilename)) {
                        shape.Template = alternateFilename;
                    }
                }
            }

            if(shape.Template == null) {
                shape.Template = descriptor.BindingSource;
            }

            if(shape.Template == null) {
                shape.Template = descriptor.Bindings.Values.FirstOrDefault().BindingSource;
            }

            if (shape.OriginalTemplate == null) {
                shape.OriginalTemplate = descriptor.Bindings.Values.FirstOrDefault().BindingSource;
            }

            try {
                // we know that templates are classes if they contain ':'
                if (!shape.Template.Contains(":") && _webSiteFolder.FileExists(shape.Template)) {
                    shape.TemplateContent = _webSiteFolder.ReadFile(shape.Template);
                }
            }
            catch {
                // the url might be invalid in case of a code shape
            }

            if (shapeMetadata.PlacementSource != null && _webSiteFolder.FileExists(shapeMetadata.PlacementSource)) {
                context.Shape.PlacementContent = _webSiteFolder.ReadFile(shapeMetadata.PlacementSource);
            }

            // Inject the Zone name
            if(shapeMetadata.Type == "Zone") {
                shape.Hint = ((Zone) shape).ZoneName;
            }

            shape.ShapeId = _shapeId++;
        }


        public void Displayed(ShapeDisplayedContext context) {
        }

        public static void ConvertToJSon(XElement x, StringBuilder sb) {
            if(x == null) {
                return;
            }

            switch (x.Name.ToString()) {
                case "ul" :
                    var first = true;
                    foreach(var li in x.Elements()) {
                        if (!first) sb.Append(",");
                        ConvertToJSon(li, sb);
                        first = false;
                    }
                    break;
                case "li":
                    var name = x.Element("h1").Value;
                    var value = x.Element("span").Value;

                    sb.AppendFormat("\"name\": \"{0}\", ", FormatJsonValue(name));
                    sb.AppendFormat("\"value\": \"{0}\"", FormatJsonValue(value));

                    var ul = x.Element("ul");
                    if (ul != null && ul.Descendants().Any()) {
                        sb.Append(", \"children\": [");
                        first = true;
                        foreach (var li in ul.Elements()) {
                            sb.Append(first ? "{ " : ", {");
                            ConvertToJSon(li, sb);
                            sb.Append(" }");
                            first = false;
                        }
                        sb.Append("]");
                    }

                    break;
            }
        }

        public static string FormatJsonValue(string value) {
            // replace " by \" in json strings
            return value.Replace(@"\", @"\\").Replace("\"", @"\""").Replace("\r\n", @"\n").Replace("\r", @"\n").Replace("\n", @"\n");
        }

        private static string FormatShapeFilename(string shape, string shapeType, string displayType, string themePrefix, string extension) {

            if (!String.IsNullOrWhiteSpace(displayType)) {
                if (shape.StartsWith(shapeType + "_" + displayType)) {
                    shape = shapeType + shape.Substring(shapeType.Length + displayType.Length + 1) + "_" + displayType;
                }
            }

            return themePrefix + "/Views/" + shape.Replace("__", "-").Replace("_", ".") + extension;
        }

    }
}
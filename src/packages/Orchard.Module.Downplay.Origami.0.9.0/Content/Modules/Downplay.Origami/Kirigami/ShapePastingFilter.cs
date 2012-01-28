using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Filters;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.Widgets.Services;
using Orchard.UI.Admin;
using Orchard.Environment.Extensions;

namespace Downplay.Origami.Kirigami
{
    /// <summary>
    /// Base somewhat on WidgetFilter; allows us to push shapes into zones based on arbitrary rules
    /// </summary>
    [OrchardFeature("Downplay.Origami.Kirigami")]
    public class ShapePastingFilter : FilterProvider, IResultFilter
    {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRuleManager _ruleManager;
        private readonly IWidgetsService _widgetsService;

        public ShapePastingFilter(
            IContentManager contentManager, 
            IWorkContextAccessor workContextAccessor, 
            IRuleManager ruleManager, 
            IWidgetsService widgetsService,
            IEnumerable<IShapeCutter> shapeCutters,
            IEnumerable<IShapePaster> shapePasters)
        {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
            _ruleManager = ruleManager;
            _widgetsService = widgetsService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Kirigami should only run on a full view rendering result
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            var workContext = _workContextAccessor.GetContext(filterContext);

            // Some standard checks
            if (workContext == null ||
                workContext.Layout == null ||
                workContext.CurrentSite == null ||
                AdminFilter.IsApplied(filterContext.RequestContext))
            {
                return;
            }

            // TODO: Build and add shape to zone.
            /*var zones = workContext.Layout.Zones;
            foreach (var widgetPart in widgetParts)
            {
                if (activeLayerIds.Contains(widgetPart.As<ICommonPart>().Container.ContentItem.Id))
                {
                    var widgetShape = _contentManager.BuildDisplay(widgetPart);
                    zones[widgetPart.Record.Zone].Add(widgetShape, widgetPart.Record.Position);
                }
            }*/

        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Themes;
using System.Web.Routing;
using Autofac;

namespace Downplay.Origami.Services
{
    public class OrigamiService : IOrigamiService
    {
        private readonly IComponentContext _context;

//        private readonly Lazy<IEnumerable<IModelDriver>> _modelDrivers;
        private readonly dynamic Shape;
        private readonly IShapeTableManager _shapeTableManager;
        private readonly Lazy<IThemeManager> _themeService;
        private readonly RequestContext _requestContext;

        public OrigamiService(
                        IComponentContext context,

  //          Lazy<IEnumerable<IModelDriver>> modelDrivers,
            IShapeFactory shapeFactory,
            Lazy<IThemeManager> themeService,
            IShapeTableManager shapeTableManager,
            RequestContext requestContext
            )
        {
//            _modelDrivers = modelDrivers;
            _context = context;


            Shape = shapeFactory;
            _shapeTableManager = shapeTableManager;
            _themeService = themeService;
            _requestContext = requestContext;

        }
        private IEnumerable<IModelDriver> _drivers;
        public IEnumerable<IModelDriver> Drivers
        {
            get
            {
                if (_drivers == null)
                    _drivers = _context.Resolve<IEnumerable<IModelDriver>>();
                return _drivers;
            }
        }


        public void BuildDisplayShape(object model, dynamic root, string displayType, string stereotype, ModelShapeContext parentContext = null)
        {
            var context = new ModelDisplayShapeContext(model, root, displayType, Shape, parentContext) { Stereotype = stereotype }; //, prefix);
            BindPlacement(context, displayType, stereotype);
            foreach (var driver in Drivers)
            {
                var result = driver.BuildDisplay(context);
                if (result != null)
                {
                    result.Apply(context);
                }
            }
            // Chain sub results?
            // They are applied to the same base object (as opposed to rendering an entirely new shape with its own zones)
            // TODO: Could be nice to chain from displays to editors and back (although then we'd always need the updater)...
            foreach (var chain in context.ChainedResults)
            {
                BuildDisplayShape(chain.Model, chain.Root ?? root, chain.DisplayType ?? displayType, stereotype, context);
                // Fire an event so parent shape can perform work after the update
                chain.OnCompleted(context);
            }
            // Done
        }

        public void BuildEditorShape(object model, dynamic root, Orchard.ContentManagement.IUpdateModel updater, string prefix, string displayType, string stereotype, ModelShapeContext parentContext = null)
        {
            var context = new ModelEditorShapeContext(model, root, Shape, prefix, displayType, updater, parentContext);
            BindPlacement(context, displayType, stereotype);
            foreach (var driver in Drivers)
            {
                ModelDriverResult result = null;
                if (updater != null)
                {
                    result = driver.UpdateEditor(context);
                }
                else {
                    result = driver.BuildEditor(context);
                }
                // Null check, there must be a performance advantage to not instancing loads of empty ModelDriverResults
                if (result != null)
                {
                    result.Apply(context);
                }
            }
            // Chain sub results?
            // They are applied to the same base object (as opposed to rendering an entirely new shape with its own zones)
            foreach (var chain in context.ChainedResults)
            {
                BuildEditorShape(chain.Model, chain.Root ?? root, updater, chain.Prefix, chain.DisplayType ?? displayType, stereotype, context);
                // Fire an event so parent shape can perform work after the update
                chain.OnCompleted(context);
            }
            // Invoke Updated event now all drivers have been executed
            if (updater != null)
            {
                context.InvokeUpdated();
            }
            // Done
        }

        private void BindPlacement(ModelShapeContext context, string displayType, string stereotype)
        {
            context.FindPlacement = (partShapeType, differentiator, defaultLocation) =>
            {
                var theme = _themeService.Value.GetRequestTheme(_requestContext);
                var shapeTable = _shapeTableManager.GetShapeTable(theme.Id);
                var request = _requestContext.HttpContext.Request;

                ShapeDescriptor descriptor;
                if (shapeTable.Descriptors.TryGetValue(partShapeType, out descriptor))
                {
                    var placementContext = new ShapePlacementContext
                    {
                        Stereotype = stereotype,
                        DisplayType = displayType,
                        Differentiator = differentiator,
                        Path = VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAppRelative(request.Path)) // get the current app-relative path, i.e. ~/my-blog/foo
                    };

                    var placement = descriptor.Placement(placementContext);
                    if (placement != null)
                    {
                        placement.Source = placementContext.Source;
                        return placement;
                    }
                }

                return new PlacementInfo
                {
                    Location = defaultLocation,
                    Source = String.Empty
                };
            };
        }

    }
}
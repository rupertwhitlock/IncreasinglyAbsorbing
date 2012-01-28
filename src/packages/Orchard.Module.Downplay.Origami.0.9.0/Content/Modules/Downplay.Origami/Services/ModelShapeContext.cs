using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Services
{
    /// <summary>
    /// </summary>
    public class ModelShapeContext
    {
        protected ModelShapeContext(object model, IShape shape, String displayType, IShapeFactory shapeFactory, ModelShapeContext parentContext)
        {
            Model = model;
            Shape = shape;
            New = shapeFactory;
            FindPlacement = (partType, differentiator, defaultLocation) => new PlacementInfo {Location = defaultLocation, Source = String.Empty};
            ParentContext = parentContext;
            DisplayType = displayType;
        }

        public object Model { get; set; }
        public dynamic Shape { get; private set; }
        public string DisplayType { get; set; }
        public string Stereotype { get; set; }

        public dynamic New { get; private set; }
        public Func<string, string, string, PlacementInfo> FindPlacement { get; set; }
        public ModelShapeContext ParentContext { get; set; }

        /// <summary>
        /// Gets all parent contexts from up the chain
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelShapeContext> ParentChain()
        {
            var currentParent = ParentContext;
            while (currentParent != null)
            {
                yield return currentParent;
                currentParent = currentParent.ParentContext;
            }
        }


    }
}
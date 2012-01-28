using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Services
{
    public class ModelDisplayShapeContext : ModelShapeContext
    {
        public List<ModelChainContext> ChainedResults { get; set; }

        public ModelDisplayShapeContext(object model, IShape root, string displayType, IShapeFactory shapeFactory, ModelShapeContext parentContext) : base(model,root,displayType,shapeFactory,parentContext)
        {
            ChainedResults = new List<ModelChainContext>();
        }
   
    }
}
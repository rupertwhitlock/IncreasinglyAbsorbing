using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Origami.Services
{

    /// <summary>
    /// Helps us with recursive rendering. Allows Origami to take over and perform a complete child model rendering process.
    /// </summary>
    public class ChildShapeResult : ModelDriverResult
    {
        private string _shapeType;
        private dynamic _shape;
        private object _model;
        private string _displayType;
        private string _prefix;

        public ChildShapeResult(string shapeType, dynamic shape, object model,string prefix, string displayType)
        {
            _shapeType = shapeType;
            _shape = shape;
            _model = model;
            _displayType = displayType;
            _prefix = prefix;
        }
        public override void Apply(ModelDisplayShapeContext context)
        {
            context.ChainedResults.Add(new ModelChainContext()
            {
                Model = _model,
                Root = _shape,
                ShapeType = _shapeType,
                DisplayType = _displayType,
                Prefix = _prefix
            });
        }
        public override void Apply(ModelEditorShapeContext context)
        {
            context.ChainedResults.Add(new ModelChainContext()
            {
                Model = _model,
                Root = _shape,
                ShapeType = _shapeType,
                DisplayType = _displayType,
                Prefix = _prefix
            });
        }
    }
}
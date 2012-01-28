using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;

namespace Downplay.Origami.Services
{
    public class ModelEditorShapeContext : ModelDisplayShapeContext
    {
        public ModelEditorShapeContext(object model, IShape shape, IShapeFactory shapeFactory, string prefix, string displayType, IUpdateModel updater = null, ModelShapeContext parentContext = null) : base(model, shape, displayType, shapeFactory, parentContext) {
            Updater = updater;
            Prefix = prefix;
        }
        public string Prefix { get; set; }

        public IUpdateModel Updater { get; set; }

        protected List<Action<ModelEditorShapeContext>> UpdatedHandlers = new List<Action<ModelEditorShapeContext>>();

        public void OnUpdated(Action<ModelEditorShapeContext> updated)
        {
            UpdatedHandlers.Add(updated);
        }
        public void InvokeUpdated()
        {
            foreach (var handler in UpdatedHandlers)
            {
                handler.Invoke(this);
            }
        }
    }
}

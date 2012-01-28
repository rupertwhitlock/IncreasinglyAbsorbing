using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement.Drivers;

namespace Downplay.Origami.Services
{
    public interface IModelDriver : IDependency
    {
        ModelDriverResult BuildDisplay(ModelDisplayShapeContext context);
        ModelDriverResult BuildEditor(ModelEditorShapeContext context);
        ModelDriverResult UpdateEditor(ModelEditorShapeContext context);
    }
}

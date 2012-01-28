using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;

namespace Downplay.Origami.Services
{
    public interface IOrigamiService : IDependency
    {
        void BuildDisplayShape(object model, dynamic root, string displayType, string stereotype, ModelShapeContext parentContext=null);
        void BuildEditorShape(object model, dynamic root, IUpdateModel updater, string prefix, string displayType, string stereotype, ModelShapeContext parentContext = null);
    }
}

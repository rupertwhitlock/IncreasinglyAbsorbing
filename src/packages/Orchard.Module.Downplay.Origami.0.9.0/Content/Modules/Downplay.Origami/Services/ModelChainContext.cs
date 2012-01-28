using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Origami.Services
{
    public class ModelChainContext
    {
        public object Model { get; set; }
        public string ShapeType { get; set; }
        public string Prefix { get; set; }
        public delegate void ChainCompleted(ModelShapeContext context);
        public event ChainCompleted Completed;

        public void OnCompleted(ModelShapeContext context)
        {
            if (Completed != null)
            {
                Completed(context);
            }
        }

        public dynamic Root { get; set; }

        public string DisplayType { get; set; }
    }
}

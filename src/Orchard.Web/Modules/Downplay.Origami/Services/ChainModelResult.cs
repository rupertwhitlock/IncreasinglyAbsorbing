using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Origami.Services
{
    public class ChainModelResult : ModelDriverResult
    {
        private Downplay.Origami.Services.ModelChainContext.ChainCompleted _onCompleted { get; set; }
        private object _model { get; set; }
        private string _prefix { get; set; }
        public ChainModelResult(object model,string prefix,Downplay.Origami.Services.ModelChainContext.ChainCompleted onCompleted)
        {
            _prefix = prefix;
            _model = model;
            _onCompleted = onCompleted;
        }

        public override void Apply(ModelDisplayShapeContext context)
        {
            var chain = new ModelChainContext()
            {
                Model = _model,
                Prefix = _prefix
            };
            if (_onCompleted!=null) {
                chain.Completed += _onCompleted;
            }
            context.ChainedResults.Add(chain);
        }

        public override void Apply(ModelEditorShapeContext context)
        {
            var chain = new ModelChainContext()
            {
                Model = _model,
                Prefix = _prefix
            };
            if (_onCompleted != null)
            {
                chain.Completed += _onCompleted;
            }
            context.ChainedResults.Add(chain);
        }

    }
}

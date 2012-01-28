using System;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Origami.Services {
    public class ModelShapeResult : ModelDriverResult {
        private string _defaultLocation;
        private string _differentiator;
        private readonly string _shapeType;
        private readonly string _prefix;
        private readonly Func<ModelShapeContext, dynamic> _shapeBuilder;
        private string _groupId;

        public ModelShapeResult(string shapeType, string prefix, Func<ModelShapeContext, dynamic> shapeBuilder)
        {
            _shapeType = shapeType;
            _prefix = prefix;
            _shapeBuilder = shapeBuilder;
        }

        public override void Apply(ModelDisplayShapeContext context) {
            ApplyImplementation(context, context.DisplayType);
        }

        public override void Apply(ModelEditorShapeContext context) {
            ApplyImplementation(context, context.DisplayType);
        }

        private void ApplyImplementation(ModelShapeContext context, string displayType)
        {
          //  if (!string.Equals(context.GroupId ?? "", _groupId ?? "", StringComparison.OrdinalIgnoreCase))
           //     return;

            var placement = context.FindPlacement(_shapeType, _differentiator, _defaultLocation);

            if (string.IsNullOrEmpty(placement.Location) || placement.Location == "-")
                return;

            dynamic parentShape = context.Shape;
            var newShape = _shapeBuilder(context);
            // Handle null shape without an error
            if (newShape == null) return;

            ShapeMetadata newShapeMetadata = newShape.Metadata;
            newShapeMetadata.Prefix = _prefix;
            // Display type might have already been set (so we're not forced to use the one from the context)
            if (string.IsNullOrWhiteSpace(newShapeMetadata.DisplayType))
            {
                newShapeMetadata.DisplayType = displayType;
            }
            newShapeMetadata.PlacementSource = placement.Source;
            
            // if a specific shape is provided, remove all previous alternates and wrappers
            if (!String.IsNullOrEmpty(placement.ShapeType)) {
                newShapeMetadata.Type = placement.ShapeType;
                newShapeMetadata.Alternates.Clear();
                newShapeMetadata.Wrappers.Clear();
            }

            foreach (var alternate in placement.Alternates) {
                newShapeMetadata.Alternates.Add(alternate);
            }

            foreach (var wrapper in placement.Wrappers) {
                newShapeMetadata.Wrappers.Add(wrapper);
            }

            var delimiterIndex = placement.Location.IndexOf(':');
            if (delimiterIndex < 0) {
                parentShape.Zones[placement.Location].Add(newShape);
            }
            else {
                var zoneName = placement.Location.Substring(0, delimiterIndex);
                var position = placement.Location.Substring(delimiterIndex + 1);
                parentShape.Zones[zoneName].Add(newShape, position);
            }
        }

        public ModelShapeResult Location(string zone) {
            _defaultLocation = zone;
            return this;
        }

        public ModelShapeResult Differentiator(string differentiator) {
            _differentiator = differentiator;
            return this;
        }

        public ModelShapeResult OnGroup(string groupId)
        {
            _groupId=groupId;
            return this;
        }
    }
}
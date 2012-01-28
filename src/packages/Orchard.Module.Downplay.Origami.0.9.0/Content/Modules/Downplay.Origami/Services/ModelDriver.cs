using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions.Models;

namespace Downplay.Origami.Services
{

    /// <summary>
    /// This is similar to Orchard's ContentPartDriver but designed for building arbitrary shapes with non-content models
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModelDriver<T> : IModelDriver
    {
        public ModelDriver() { }

        protected abstract String Prefix { get; }

        ModelDriverResult IModelDriver.BuildDisplay(ModelDisplayShapeContext context)
        {
            if (context.Model is T)
            {
                return Display((T)context.Model, context.New, context);
            }
            return null;
        }

        ModelDriverResult IModelDriver.UpdateEditor(ModelEditorShapeContext context)
        {
            if (context.Model is T)
            {
                return Update((T)context.Model, context.New, context.Updater, context);
            }
            return null;
        }

        ModelDriverResult IModelDriver.BuildEditor(ModelEditorShapeContext context)
        {
            if (context.Model is T)
            {
                return Editor((T)context.Model, context.New, context);
            }
            return null;
        }

        protected abstract ModelDriverResult Display(T model, dynamic shapeHelper, ModelDisplayShapeContext context);
        protected abstract ModelDriverResult Editor(T model, dynamic shapeHelper, ModelEditorShapeContext context);
        protected abstract ModelDriverResult Update(T model, dynamic shapeHelper, IUpdateModel updater, ModelEditorShapeContext context);

        public ModelDriverResult ModelShape(string shapeType, string differentiator, dynamic shape)
        {
            // NOTE: Even though we're passing the wrong Prefix in here, i.e. not FullPrefix, and it ends up in ShapeMetadata; it doesn't get referenced from ANYwhere
            // that I could find so maybe it's not a problem...
            return ContentShapeImplementation(shapeType, Prefix, ctx => shape).Differentiator(differentiator);
        }

        public ModelDriverResult EditorShape(string shapeType, object model, ModelEditorShapeContext context)
        {
            return ContentShapeImplementation(shapeType, FullPrefix(context), (ctx) => context.New.EditorTemplate(TemplateName: shapeType.Replace('_', '.'), Model: model, Prefix:FullPrefix(context)));
        }

        public ModelDriverResult ModelShape(string shapeType, dynamic shape)
        {
            return ContentShapeImplementation(shapeType, Prefix, ctx => shape);
        }

        public ModelDriverResult ModelShape(string shapeType, Func<dynamic> factory)
        {
            return ContentShapeImplementation(shapeType, Prefix, ctx => factory());
        }

        public ModelDriverResult ModelShape(string shapeType, Func<dynamic, dynamic> factory)
        {
            return ContentShapeImplementation(shapeType, Prefix, ctx => factory(CreateShape(ctx, shapeType)));
        }

        public ModelDriverResult ChainShape(object chainedModel, string prefix, Action complete)
        {
            return new ChainModelResult(chainedModel, prefix, ctx => complete());
        }
        protected ModelDriverResult ChildShape(string shapeType, dynamic shape, object model, string prefix, string displayType)
        {
            return new ChildShapeResult(shapeType, shape, model, prefix, displayType);
        }

        private ModelShapeResult ContentShapeImplementation(string shapeType, string prefix, Func<ModelShapeContext, object> shapeBuilder)
        {
            return new ModelShapeResult(shapeType, prefix, shapeBuilder);
        }

        private static object CreateShape(ModelShapeContext context, string shapeType)
        {
            IShapeFactory shapeFactory = context.New;
            return shapeFactory.Create(shapeType);
        }

        public CombinedModelResult Combined(params ModelDriverResult[] results)
        {
            return new CombinedModelResult(results);
        }

        /// <summary>
        /// Build prefix for template, using parent prefix if available
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public String FullPrefix(ModelEditorShapeContext context,string fieldName=null)
        {
            var prefix = Prefix;
            if (!String.IsNullOrWhiteSpace(context.Prefix))
            {
                prefix = context.Prefix + "." + prefix;
            }
            if (!String.IsNullOrWhiteSpace(fieldName))
            {
                prefix = prefix + "." + fieldName;
            }
            return prefix;
        }

    }

    public abstract class ModelDriver<TModel, TViewModel> : ModelDriver<TModel>
    {
        public ModelDriver() { }

        public abstract TViewModel ViewModel(TModel model, ModelShapeContext context);

        protected override ModelDriverResult Display(TModel model, dynamic shapeHelper, ModelDisplayShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Display(model, viewModel, shapeHelper, context);
        }

        protected virtual ModelDriverResult Display(TModel model, TViewModel viewModel, dynamic shapeHelper, ModelDisplayShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(TModel model, dynamic shapeHelper, ModelEditorShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Editor(model, viewModel, shapeHelper, context);
        }

        protected virtual ModelDriverResult Editor(TModel model, TViewModel viewModel, dynamic shapeHelper, ModelEditorShapeContext context)
        {
            // By default we only have to implement Update and always check whether updater != null ...
            return Update(model, viewModel, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(TModel model, dynamic shapeHelper, IUpdateModel updater, ModelEditorShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Update(model, viewModel, shapeHelper, updater, context);
        }

        protected virtual ModelDriverResult Update(TModel model, TViewModel viewModel, dynamic shapeHelper, IUpdateModel updater, ModelEditorShapeContext context)
        {
            return null;
        }

    }

}
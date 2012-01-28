using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami;
using MediaGarden.Models;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;
using Orchard.ContentManagement;

namespace MediaGarden.Pipeline
{
    /// <summary>
    /// Base driver for providing edit and other controls on individual sources,
    /// to perform additional filtering / processing during import.
    /// </summary>
    public abstract class MediaSourceDriver<T> : ModelDriver<MediaSourceViewModel,T>
    {

    }
    public abstract class MediaSourceDriver<T, TPart> : MediaSourceDriver<T>, IMediaContentFilter
        where TPart : IContent
    {

        public override T ViewModel(MediaSourceViewModel model, ModelShapeContext context)
        {
            return ViewModel(model.Source);
        }

        protected abstract T ViewModel(IMediaSource source);

        public void ContentFiltering(MediaCreateContext context)
        {
            var part = context.ContentItem.As<TPart>();
            if (part != null)
            {
                var viewModel = ViewModel(context.Source);
                Apply(part, viewModel, context.Source, context);
            }
        }

        protected abstract void Apply(TPart part, T model, IMediaSource source, MediaCreateContext context);

        public void ContentFiltered(MediaCreateContext context)
        {
            var part = context.ContentItem.As<TPart>();
            if (part != null)
            {
                var viewModel = ViewModel(context.Source);
                Applied(part, viewModel, context.Source, context);
            }
        }

        protected abstract void Applied(TPart part, T model, IMediaSource source, MediaCreateContext context);
    }

}
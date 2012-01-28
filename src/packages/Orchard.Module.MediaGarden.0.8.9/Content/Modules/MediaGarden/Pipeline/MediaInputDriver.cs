using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Downplay.Origami;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;

namespace MediaGarden.Pipeline
{
    public abstract class MediaInputDriver<T> : ModelDriver<MediaInputsViewModel,T>
        where T:class, new()
    {
        public override T ViewModel(MediaInputsViewModel model, ModelShapeContext context)
        {
            return new T();
        }
    }
}
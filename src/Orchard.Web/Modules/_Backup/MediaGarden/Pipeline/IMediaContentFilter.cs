using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace MediaGarden.Pipeline
{
    public interface IMediaContentFilter : IDependency
    {

        void ContentFiltering(MediaCreateContext context);
        void ContentFiltered(MediaCreateContext context);

    }
}
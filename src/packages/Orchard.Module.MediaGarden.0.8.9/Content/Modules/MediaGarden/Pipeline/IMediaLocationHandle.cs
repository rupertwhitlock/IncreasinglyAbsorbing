using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaGarden.Pipeline
{
    public interface IMediaLocationHandle
    {
        MediaHeaderContext AcquireHeaders();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MediaGarden.Pipeline
{
    public interface IStreamAccessor
    {
        Stream GetStream();
    }
}

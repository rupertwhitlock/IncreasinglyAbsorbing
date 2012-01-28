using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources.FileSystem
{
    public class FileSystemStreamAccessor : IStreamAccessor
    {
        private System.IO.Stream stream;

        public FileSystemStreamAccessor(System.IO.Stream stream)
        {
            // TODO: Complete member initialization
            this.stream = stream;
        }

        public System.IO.Stream GetStream()
        {
            return stream;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Pipeline;

namespace MediaGarden.Sources.FileSystem
{
    public class FileSystemLocationContext : MediaLocationContext
    {
        public Orchard.FileSystems.Media.IStorageFile File { get; set; }
    }
}

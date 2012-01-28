using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Media.Services;
using Orchard;
using MediaGarden.Pipeline;
using Orchard.FileSystems.Media;

namespace MediaGarden.Sources.FileSystem
{
    public class FileSystemQueryFilter : IMediaQueryFilter
    {
        
        private readonly IOrchardServices _services;
        private readonly IMediaService _orchardMediaService;
        private readonly IStorageProvider _storageProvider;
 
        /// <summary>
        /// Injecting the default orchard media service and storage provider for file handling
        /// </summary>
        /// <param name="services"></param>
        /// <param name="orchardMediaService"></param>
        public FileSystemQueryFilter(
            IOrchardServices services, 
            IMediaService orchardMediaService,
            IStorageProvider storageProvider)
        {
            _services = services;
            // Poss don't need to use this; it's only a thin wrapper around IStorageProvider anyway
            _orchardMediaService = orchardMediaService;
            _storageProvider = storageProvider;
        }

        public void QueryFiltering(MediaQueryContext context)
        {
            // Opposite of URL location filter
            if (context.Query.StartsWith("http://")
                || context.Query.StartsWith("https://")) return;
            var paths = new List<String>();
            if (context.Query== "*")
            {
                paths.Add(null);
            }
            else
            {
                paths.AddRange(context.Query.Split(';'));
            }
            List<IStorageFile> files = new List<IStorageFile>();
            foreach (var folder in paths)
            {
                // Currently we can only use try/catch because *there's no "file exists" method*!!
                try
                {
                    var storageFiles = _storageProvider.ListFiles(folder);
                    files.AddRange(storageFiles);
                }
                catch
                {
                    try
                    {
                        // That wasn't a folder. Let's try getting a single file.
                        var file = _storageProvider.GetFile(folder);
                        files.Add(file);
                    }
                    catch
                    {
                        // No files
                    }
                }
            }
            foreach (var file in files)
            {
                string filePath = file.GetPath();

                context.Locations.Add(new FileSystemLocationContext()
                {
                    QueryName = "FileSystem",
                    Location = filePath,
                    File = file,
                    Stream = new FileSystemStreamAccessor(file.OpenRead())
                });
            }
        }

        public void QueryFiltered(MediaQueryContext context)
        {
        }
    }
}
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MediaGarden.Models;

namespace MediaGarden.Handlers
{
    [UsedImplicitly]
    public class FileSizePartHandler : ContentHandler
    {
        public FileSizePartHandler(IRepository<FileSizePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

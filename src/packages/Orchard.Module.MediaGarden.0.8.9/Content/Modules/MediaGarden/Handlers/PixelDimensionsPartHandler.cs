using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MediaGarden.Models;

namespace MediaGarden.Handlers
{
    [UsedImplicitly]
    public class PixelDimensionsPartHandler : ContentHandler
    {
        public PixelDimensionsPartHandler(IRepository<PixelDimensionsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

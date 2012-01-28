using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MediaGarden.Models;

namespace MediaGarden.Handlers
{
    [UsedImplicitly]
    public class MediaLengthPartHandler : ContentHandler
    {
        public MediaLengthPartHandler(IRepository<MediaLengthPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

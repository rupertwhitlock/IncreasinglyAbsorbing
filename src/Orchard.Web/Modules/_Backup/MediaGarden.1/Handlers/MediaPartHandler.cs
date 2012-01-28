using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MediaGarden.Models;
using MediaGarden.Services;
using System.Linq;

namespace MediaGarden.Handlers
{
    [UsedImplicitly]
    public class MediaPartHandler : ContentHandler
    {
        private readonly IMediaGardenService _gardenService;

        public MediaPartHandler(IRepository<MediaPartRecord> repository, IMediaGardenService gardenService)
        {
            _gardenService = gardenService;

            Filters.Add(StorageFilter.For(repository));
            OnLoaded<MediaPart>((context, part) =>
            {
                // TODO: Reduce reliance on Source, actually there should be no direct reference like this needed
                if (part.Source != null)
                {
                    part.MediaSource = _gardenService.MediaSourceFromRecord(part.Source);
                }
                part.MediaFormat = _gardenService.GetFormat(part.FormatName);
            
            });
            OnGetDisplayShape<MediaPart>((context, part) =>
                {

                    part.MediaViewer = _gardenService.FindViewer(part, part.ViewerName);
            //        context.Shape.MediaUrl = _gardenService.AbsoluteMediaUrl(part);

                });
        }
    }
}

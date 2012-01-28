using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.jPlayer.Models;

namespace Orchard.jPlayer.Handlers {
    public class MediaGalleryHandler : ContentHandler {
        public MediaGalleryHandler(IRepository<MediaGalleryRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
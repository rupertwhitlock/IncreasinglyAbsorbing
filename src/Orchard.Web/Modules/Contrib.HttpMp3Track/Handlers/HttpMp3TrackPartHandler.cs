using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contrib.HttpMp3Track.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Contrib.HttpMp3Track.Handlers
{
    public class HttpMp3TrackPartHandler : ContentHandler
    {
        public HttpMp3TrackPartHandler(IRepository<HttpMp3TrackPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

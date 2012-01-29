using System.Collections.Generic;
using Orchard;
using Rupert.GetMeThatRecord.Models;

namespace Rupert.GetMeThatRecord.Services
{
    public interface IRecordWebsiteTrackSamplesService : IDependency {
        IEnumerable<string> ExtractTracksFromUrl(ThatRecordPart thatRecordPart);
    }
}

using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;

namespace Rupert.GetMeThatRecord.Services
{
    public interface IThatRecordService : IDependency {
        void UpdateTracksForThatRecord(ContentItem item, IList<string> tracks);
    }
}

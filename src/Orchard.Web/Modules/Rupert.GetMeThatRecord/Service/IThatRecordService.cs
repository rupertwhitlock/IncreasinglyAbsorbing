using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;

namespace Rupert.GetMeThatRecord.Service
{
    public interface IThatRecordService : IDependency {
        void UpdateTracksForThatRecord(ContentItem item, IList<string> tracks);
    }
}

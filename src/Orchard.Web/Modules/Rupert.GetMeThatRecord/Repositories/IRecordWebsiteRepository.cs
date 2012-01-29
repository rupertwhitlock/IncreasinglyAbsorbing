using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Rupert.GetMeThatRecord.Enums;
using Rupert.GetMeThatRecord.Models;

namespace Rupert.GetMeThatRecord.Repositories
{
    public interface IRecordWebsiteRepository : IDependency {
        void Load(ThatRecordPart thatRecordPart);
        RecordWebsite GetRecordWebsite(string url);
    }
}

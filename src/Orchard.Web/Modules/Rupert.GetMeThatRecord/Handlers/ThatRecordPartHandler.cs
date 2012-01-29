using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Rupert.GetMeThatRecord.Models;

namespace Rupert.GetMeThatRecord.Handlers
{
    public class ThatRecordPartHandler : ContentHandler
    {
        public ThatRecordPartHandler(IRepository<ThatRecordPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

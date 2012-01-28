using System.Collections.Generic;
using Orchard.ContentManagement;

namespace MediaGarden.ViewModels {
    public class ListSourcesViewModel  {
        public ListSourcesViewModel()
        {
            Options = new ContentOptions();
        }

        public string Id { get; set; }

        public string TypeName {
            get { return Id; }
        }

        public string TypeDisplayName { get; set; }
        public int? Page { get; set; }
        public IList<Entry> Entries { get; set; }
        public ContentOptions Options { get; set; }
        public int? MediaSessionId { get; set; }
        #region Nested type: Entry

        public class Entry {
            public ContentItem ContentItem { get; set; }
            public ContentItemMetadata ContentItemMetadata { get; set; }
        }

        #endregion
    }

    public enum SourcesBulkAction
    {
        None,
        Import
    }
}
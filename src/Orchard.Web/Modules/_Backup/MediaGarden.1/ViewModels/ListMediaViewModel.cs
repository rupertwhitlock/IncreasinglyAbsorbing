﻿using System.Collections.Generic;
using Orchard.ContentManagement;

namespace MediaGarden.ViewModels {
    public class ListMediaViewModel  {
        public ListMediaViewModel()
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

        #region Nested type: Entry

        public class Entry {
            public ContentItem ContentItem { get; set; }
            public ContentItemMetadata ContentItemMetadata { get; set; }
        }

        #endregion
    }

    public class ContentOptions {
        public ContentOptions() {
            OrderBy = ContentsOrder.Modified;
            BulkAction = ContentsBulkAction.None;
        }
        public string SelectedFilter { get; set; }
        public IEnumerable<KeyValuePair<string, string>> FilterOptions { get; set; }
        public ContentsOrder OrderBy { get; set; }
        public ContentsBulkAction BulkAction { get; set; }
    }

    public enum ContentsOrder {
        Modified,
        Published,
        Created
    }

    public enum ContentsBulkAction {
        None,
        PublishNow,
        Unpublish,
        Remove
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaGarden.Models;
using Orchard.ContentManagement.MetaData.Models;

namespace MediaGarden.Models
{
    /// <summary>
    /// Represents each piece of data returned from a source
    /// </summary>
    public interface IMediaSource
    {
        int RecordId { get; }
        string Url { get; }
        string MediaStereotype { get; }
        SettingsDictionary Metadata { get; set; }

        /// <summary>
        /// Part of the puzzle of checking for updated / new items?
        /// </summary>
        DateTime TimeStamp { get; }
        MediaSourceRecord Record { get; }

        string Title { get; set; }
    }
}

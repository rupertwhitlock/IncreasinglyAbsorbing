using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;

namespace MediaGarden.Models
{
    public class MediaSource : IMediaSource
    {
        public MediaSource()
        {
            Metadata = new Orchard.ContentManagement.MetaData.Models.SettingsDictionary();
        }
        public MediaSource(MediaSourceRecord record)
        {
            _Record = record;
        }
        private MediaSourceRecord _Record { get; set; }
        public MediaSourceRecord Record
        {
            get
            {
                if (_Record == null)
                {
                    _Record = new MediaSourceRecord();
                }
                return _Record;
            }
        }

        public int RecordId { get { return Record.Id; } }
        public String Url { get { return Record.Location; } set { Record.Location = value; } }
        public string Title
        {
            get
            {
                return Metadata["Title"];
            }
            set
            {
                Metadata["Title"] = value;
            }
        }
        public string MediaStereotype { get { return Record.MediaStereotype; } set { Record.MediaStereotype = value; } }
        public string FormatName { get { return Record.FormatName; } set { Record.FormatName = value; } }
        public DateTime TimeStamp { get { return Record.LastModified; } set { Record.LastModified = value; } }
        public Orchard.ContentManagement.MetaData.Models.SettingsDictionary Metadata {get ; set; }
    }   
}
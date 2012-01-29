using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Orchard.ContentManagement;
using Rupert.GetMeThatRecord.Enums;
using Rupert.GetMeThatRecord.Models;
using Rupert.GetMeThatRecord.Settings;

namespace Rupert.GetMeThatRecord.Repositories
{
    public class RecordWebsiteRepository : IRecordWebsiteRepository
    {
        private readonly IContentManager _contentManager;
        private IList<KeyValuePair<string, RecordWebsite>> RecordWebsites { get; set; }
        

        public RecordWebsiteRepository(
            IContentManager contentManager) {
            _contentManager = contentManager;
            RecordWebsites = new List<KeyValuePair<string, RecordWebsite>>();
        }

        public void Load(ThatRecordPart thatRecordPart) {

            var settings = thatRecordPart.Settings.GetModel<ThatRecordPartSettings>();

            var xmlPath = thatRecordPart.Settings.GetModel<ThatRecordPartSettings>().XmlFilePathForRecordWebsiteUrls;
            var xmlContent = File.ReadAllText(xmlPath);
            var xDocument = XDocument.Parse(xmlContent);

            try
            {
                RecordWebsites = xDocument.Root.Elements()
                    .Select(x => new KeyValuePair<string, RecordWebsite>(x.Attribute("url").Value, (RecordWebsite)Enum.Parse(typeof(RecordWebsite), x.Attribute("name").Value, true)))
                    .ToList();
            }
            catch (NullReferenceException e)
            {
                return;
            }
        }

        public RecordWebsite GetRecordWebsite(string url) {

            var match = RecordWebsites.Where(website => Regex.IsMatch(url, website.Key, RegexOptions.IgnoreCase));

            return match.Count() > 0 ? match.Single().Value : RecordWebsite.Unknown;
        }
    }
}

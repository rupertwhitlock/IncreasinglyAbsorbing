using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HtmlAgilityPack;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Rupert.GetMeThatRecord.Models;
using Rupert.GetMeThatRecord.Service;

namespace Rupert.GetMeThatRecord.Drivers
{
    public class ThatRecordPartDriver : ContentPartDriver<ThatRecordPart>
    {
        private readonly IThatRecordService _thatRecordService;
        

        public ThatRecordPartDriver(IThatRecordService thatRecordService) {
            _thatRecordService = thatRecordService;
        }

        protected override string Prefix
        {
            get
            {
                return "ThatRecord";
            }
        }

        protected override DriverResult Display(ThatRecordPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_ThatRecord",
                                () => shapeHelper.Parts_ThatRecord(
                                    Tracks: part.Tracks.Select(x => x.TrackUrl)));
        }


        protected override DriverResult Editor(ThatRecordPart part, dynamic shapeHelper) {
            return ContentShape("Parts_ThatRecord_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/ThatRecord",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(ThatRecordPart part, IUpdateModel updater, dynamic shapeHelper) {
            if(updater.TryUpdateModel(part, Prefix, null, null)) {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(part.RecordUrl); 
                var soundsSamples = htmlDoc.DocumentNode.SelectNodes("//div[@id='vpSoundSamples']/a[@href]");
                var soundSampleUrls = soundsSamples.Select(x => Regex.Match(x.Attributes["href"].Value, "http://[0-9./a-zA-z]+.mp3").Value);

                if(part.ContentItem.Id != 0) {
                    _thatRecordService.UpdateTracksForThatRecord(
                        part.ContentItem, soundSampleUrls.ToList());
                }
            }
            return Editor(part, shapeHelper);
        }
    }
}

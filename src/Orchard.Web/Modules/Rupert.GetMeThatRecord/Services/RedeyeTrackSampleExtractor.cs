using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Rupert.GetMeThatRecord.Services
{
    public class RedeyeTrackSampleExtractor : ITrackSampleExtractor
    {
        public IEnumerable<string> ExtractTrackSamples(string recordUrl) {

            var web = new HtmlWeb();
            var htmlDoc = web.Load(recordUrl);
            var soundsSamples = htmlDoc.DocumentNode.SelectNodes("//div[@id='vpSoundSamples']/a[@href]");
            return soundsSamples.Select(x => Regex.Match(x.Attributes["href"].Value, "http://[0-9./a-zA-z]+.mp3").Value);

        }
    }
}

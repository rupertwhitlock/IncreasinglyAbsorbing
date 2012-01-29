using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rupert.GetMeThatRecord.Enums;
using Rupert.GetMeThatRecord.Services;

namespace Rupert.GetMeThatRecord.Factories
{
    public static class TrackSampleExtractorFactory
    {
        public static ITrackSampleExtractor Create(RecordWebsite recordWebsite) {
            switch(recordWebsite) {
                case RecordWebsite.Unknown:
                    return null;
                case RecordWebsite.Redeye:
                    return new RedeyeTrackSampleExtractor();
                default:
                    return null;
            }

        }

    }
}

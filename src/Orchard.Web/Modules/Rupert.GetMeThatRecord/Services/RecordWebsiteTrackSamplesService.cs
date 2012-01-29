using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rupert.GetMeThatRecord.Enums;
using Rupert.GetMeThatRecord.Models;
using Rupert.GetMeThatRecord.Repositories;
using Rupert.GetMeThatRecord.Factories;

namespace Rupert.GetMeThatRecord.Services
{
    public class RecordWebsiteTrackSamplesService : IRecordWebsiteTrackSamplesService
    {
        private readonly IRecordWebsiteRepository _recordWebsiteRepository;

        public RecordWebsiteTrackSamplesService(
            IRecordWebsiteRepository recordWebsiteRepository) {
            _recordWebsiteRepository = recordWebsiteRepository;
        }

        public IEnumerable<string> ExtractTracksFromUrl(ThatRecordPart thatRecordPart){

            _recordWebsiteRepository.Load(thatRecordPart);
            var website = _recordWebsiteRepository.GetRecordWebsite(thatRecordPart.RecordUrl);

            if(website==RecordWebsite.Unknown) {
                return Enumerable.Empty<string>();
            }

            var extractor = TrackSampleExtractorFactory.Create(website);
            if(extractor == null) {
                return Enumerable.Empty<string>();
            }

            return extractor.ExtractTrackSamples(thatRecordPart.RecordUrl);
        }
    }
}

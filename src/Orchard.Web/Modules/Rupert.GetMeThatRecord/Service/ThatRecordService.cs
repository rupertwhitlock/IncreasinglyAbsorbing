using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Orchard.Data;
using Rupert.GetMeThatRecord.Models;

namespace Rupert.GetMeThatRecord.Service
{
    public class ThatRecordService : IThatRecordService
    {
        private readonly IRepository<TrackRecord> _trackRecordRepository;
        private readonly IRepository<ThatRecordTrackRecord> _thatRecordTrackRecordRepository;

        public ThatRecordService(
            IRepository<TrackRecord> trackRecordRepository,
            IRepository<ThatRecordTrackRecord> thatRecordTrackRecordRepository) {
            _trackRecordRepository = trackRecordRepository;
            _thatRecordTrackRecordRepository = thatRecordTrackRecordRepository;
        }

        public void UpdateTracksForThatRecord(ContentItem item, IList<string> tracks) {

            var record = item.As<ThatRecordPart>().Record;
            var oldRecordTracks = GetOldRecordTracks(record);

            // Delete old tracks that are no longer there

            foreach (var oldRecordTrack in oldRecordTracks)
            {
                ThatRecordTrackRecord oldRecordTrackRecord = oldRecordTrack;
                if (tracks.All(x => x != oldRecordTrackRecord.TrackRecord.TrackUrl)) {
                    TrackRecord trackRecord = oldRecordTrackRecord.TrackRecord;
                    _thatRecordTrackRecordRepository.Delete(oldRecordTrackRecord);
                    _trackRecordRepository.Delete(trackRecord);
                }
            }

            // Add new tracks
            oldRecordTracks = GetOldRecordTracks(record);

            foreach (var newTrackUrl in tracks) {
                string url = newTrackUrl;
                if(oldRecordTracks.All(oldRecordTrack => oldRecordTrack.TrackRecord.TrackUrl != url)) {
                    _trackRecordRepository.Create(new TrackRecord {
                        TrackUrl = url
                    });

                    var trackRecord = _trackRecordRepository.Get(x => x.TrackUrl == url);

                    _thatRecordTrackRecordRepository.Create(new ThatRecordTrackRecord {
                        TrackRecord = trackRecord,
                        ThatRecordPartRecord = record
                    });
                }
            }
        }

        private IEnumerable<ThatRecordTrackRecord> GetOldRecordTracks(ThatRecordPartRecord record) {
            IEnumerable<ThatRecordTrackRecord> oldRecordTracks = _thatRecordTrackRecordRepository
                .Fetch(r => r.ThatRecordPartRecord == record);
            return oldRecordTracks;
        }
    }
}

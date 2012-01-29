using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rupert.GetMeThatRecord.Services
{
    public interface ITrackSampleExtractor {
        IEnumerable<string> ExtractTrackSamples(string recordUrl);
    }
}

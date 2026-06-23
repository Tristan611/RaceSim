using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TimeResult : IResult
    {
        public string ParticipantName { get; set; }
        public TimeSpan Time { get; set; }

        public TimeResult(string participantName, TimeSpan time)
        {
            ParticipantName = participantName;
            Time = time;
        }

        public void Add(List<IResult> results)
        {
            results.Add(this);
        }

        public string GetBestParticipant(List<IResult> results)
        {
            return results
                .OfType<TimeResult>()
                .OrderBy(result => result.Time)
                .FirstOrDefault()?.ParticipantName ?? string.Empty;
        }
    }
}

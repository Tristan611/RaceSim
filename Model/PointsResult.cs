using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PointsResult : IResult
    {
        public string ParticipantName { get; set; }
        public int Points { get; set; }

        public PointsResult(string participantName, int points)
        {
            ParticipantName = participantName;
            Points = points;
        }

        public void Add(List<IResult> results)
        {
            var existingResult = results
                .OfType<PointsResult>()
                .FirstOrDefault(result => result.ParticipantName == ParticipantName);

            if (existingResult == null)
            {
                results.Add(this);
            }
            else
            {
                existingResult.Points += Points;
            }
        }

        public string GetBestParticipant(List<IResult> results)
        {
            return results
                .OfType<PointsResult>()
                .OrderByDescending(result => result.Points)
                .FirstOrDefault()?.ParticipantName ?? string.Empty;
        }
    }
}

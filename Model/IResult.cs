using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IResult
    {
        string ParticipantName { get; set; }

        void Add(List<IResult> results);

        string GetBestParticipant(List<IResult> results);
    }
}

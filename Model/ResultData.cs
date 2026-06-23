using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;       
using Model;

namespace Model
{
    public class ResultData<T> where T : IResult
    {
        private List<IResult> _results = new List<IResult>();

        public IReadOnlyList<IResult> Results => _results;

        public void Add(T result)
        {
            result.Add(_results);
        }

        public string GetBestParticipant()
        {
            if (!_results.Any())
            {
                return string.Empty;
            }

            return _results.First().GetBestParticipant(_results);
        }
    }
}

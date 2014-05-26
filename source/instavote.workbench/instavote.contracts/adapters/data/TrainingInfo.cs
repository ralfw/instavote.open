using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instavote.contracts.adapters.data
{
    public class TrainingInfo
    {
        public string Id;
        public string Matchcode;
        public VoteInfo[] Votes;
        public double AverageScore;
    }

    public class VoteInfo
    {
        public string Name;
        public string Email;
        public int Score;
        public string Suggestions;
    }
}

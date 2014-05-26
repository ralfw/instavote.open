using System;
using System.Dynamic;
using System.Linq;
using instavote.contracts.adapters;
using instavote.contracts.adapters.data;

namespace instavote.dialogs
{
    public class TrainingDialog : ITrainingDialog
    {
        private readonly NancyPortal _portal;

        public TrainingDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/training/{TrainingId}"] = _ =>
            {
                OnShowRequest(_.TrainingId);
                return _portal.SharedResponse;
            };
        }


        public void Show(TrainingInfo info)
        {
            dynamic vm = new ExpandoObject();
            vm.TrainingMatchcode = info.Matchcode;
            var i = 0;
            vm.Votes = info.Votes.Select(vi => new {Pos = ++i, vi.Name, vi.Email, vi.Score}).ToArray();
            vm.AverageScore = info.AverageScore;
            vm.Suggestions = info.Votes.Where(vi=>vi.Suggestions != "").Select(vi => vi.Suggestions + " (" + vi.Name + ")").ToArray();
            vm.Mailinglist = string.Join(", ", info.Votes.Select(vi => vi.Email));
            _portal.SharedResponse = _portal.View["Training.Main.html", vm];
        }


        public event Action<string> OnShowRequest;
    }

}

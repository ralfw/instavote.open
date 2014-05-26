using System;
using System.Dynamic;
using System.Linq;
using instavote.contracts.adapters;

namespace instavote.dialogs
{
    public class TrainingsübersichtDialog : ITrainingsübersichtDialog
    {
        private readonly NancyPortal _portal;

        public TrainingsübersichtDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/trainings/{TrainerMatchcode}"] = _ =>
            {
                OnShowRequest(_.TrainerMatchcode);
                return _portal.SharedResponse;
            };

            _portal.Post["/trainings/{TrainerMatchcode}"] = _ => {
                OnTrainingCreationRequest(_.TrainerMatchcode, _portal.Request.Form.NewTrainingMatchcode);
                return _portal.SharedResponse;
            };
        }


        public void Show(string trainerMatchcode, Tuple<string,string>[] trainings, string error)
        {
            dynamic vm = new ExpandoObject();
            vm.TrainerMatchcode = trainerMatchcode;
            vm.Trainings = trainings.Select(tr => new {Id = tr.Item1, Matchcode = tr.Item2}).ToArray();
            vm.Error = error;
            _portal.SharedResponse = _portal.View["Trainingsuebersicht.Main.html", vm];
        }


        public event Action<string> OnShowRequest;
        public event Action<string, string> OnTrainingCreationRequest;
    }
}

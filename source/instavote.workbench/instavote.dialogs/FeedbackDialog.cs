using System;
using Nancy.ModelBinding;
using instavote.contracts.adapters;

namespace instavote.dialogs
{


    public class FeedbackDialog : IFeedbackDialog
    {
        public class FeedbackVM
        {
            public string TrainingMatchcode { get; set; }
            public string Email { get; set; }
            public int Score { get; set; }
            public string Suggestions { get; set; }
            public string Error { get; set; }
        }


        private readonly NancyPortal _portal;

        public FeedbackDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/feedback/{TrainingMatchcode}"] = _ => {
                OnShowRequest(_.TrainingMatchcode);
                return _portal.SharedResponse;
            };

            _portal.Post["/feedback"] = _ =>
            {
                var vm = _portal.Bind<FeedbackVM>();
                OnFeedbackRequest(vm.TrainingMatchcode, vm.Email, vm.Score, vm.Suggestions);
                return _portal.SharedResponse;
            };
        }

        private void Render(FeedbackVM vm)
        {
            _portal.SharedResponse = _portal.View["Feedback.Main.html", vm];
        }


        public void Show(string trainingMatchcode)
        {
            var vm = new FeedbackVM{ TrainingMatchcode = trainingMatchcode };
            Render(vm);
        }

        public void Retry(string trainingMatchcode, string email, int score, string suggestions, string error)
        {
            var vm = new FeedbackVM { TrainingMatchcode = trainingMatchcode, Email = email, Score = score, Suggestions = suggestions, Error = error };
            Render(vm);
        }

        public void Ack(string trainingMatchcode)
        {
            var vm = new FeedbackVM { TrainingMatchcode = trainingMatchcode };
            _portal.SharedResponse = _portal.View["Feedback.Ack.html", vm];
        }


        public event Action<string> OnShowRequest;
        public event Action<string, string, int, string> OnFeedbackRequest;
    }
}

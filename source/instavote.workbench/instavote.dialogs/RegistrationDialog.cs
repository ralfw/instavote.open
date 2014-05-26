using System;
using Nancy.ModelBinding;
using instavote.contracts.adapters;

namespace instavote.dialogs
{
    public class RegistrationDialog : IRegistrationDialog
    {
        public class RegistrationVM
        {
            public string TrainingMatchcode { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Error { get; set; }
        }


        private readonly NancyPortal _portal;

        public RegistrationDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/register/{TrainingMatchcode}"] = _ => {
                OnShowRequest(_.TrainingMatchcode);
                return _portal.SharedResponse;
            };

            _portal.Post["/register"] = _ => {
                var vm = _portal.Bind<RegistrationVM>();
                OnRegistrationRequest(vm.TrainingMatchcode, vm.Name, vm.Email);
                return _portal.SharedResponse;
            };
        }

        private void Render(RegistrationVM vm)
        {
            _portal.SharedResponse = _portal.View["Registration.Main.html", vm];
        }



        public void Show(string trainingMatchcode)
        {
            var vm = new RegistrationVM {TrainingMatchcode = trainingMatchcode};
            Render(vm);
        }


        public void Retry(string trainingMatchcode, string name, string email, string error)
        {
            var vm = new RegistrationVM { TrainingMatchcode = trainingMatchcode, Name = name, Email = email, Error = error};
            Render(vm);
        }


        public void Ack(string trainingMatchcode, string name, string email)
        {
            var vm = new RegistrationVM { TrainingMatchcode = trainingMatchcode, Name = name, Email = email };
            _portal.SharedResponse = _portal.View["Registration.Ack.html", vm];
        }


        public event Action<string> OnShowRequest;
        public event Action<string, string, string> OnRegistrationRequest;
    }
}

using System;

namespace instavote.contracts.adapters
{
    public interface ITrainingsübersichtDialog
    {
        void Show(string trainerMatchcode, Tuple<string,string>[] trainings, string error);
        event Action<string> OnShowRequest;
        event Action<string, string> OnTrainingCreationRequest;
    }
}
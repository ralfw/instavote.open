using System;
using instavote.contracts.domaindata;

namespace instavote.contracts.adapters
{
    public interface IRepository
    {
        void LoadTrainingByMatchcode(string matchcode, Action<Training> onSucces, Action<Exception> onFailure);
        void LoadTrainingById(string id, Action<Training> onSucces, Action<Exception> onFailure);

        void UpdateTraining(Training training, Action onSucces, Action<Exception> onFailure);

        void CreateTraining(string matchcode, string trainerMatchcode, Action<Training> onSuccess, Action<Exception> onFailure);
        Training[] LoadTrainings(string trainerMatchcode);
    }
}
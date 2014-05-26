using System;
using instavote.contracts.adapters.data;

namespace instavote.contracts.adapters
{
    public interface ITrainingDialog
    {
        void Show(TrainingInfo info);
        event Action<string> OnShowRequest;
    }
}
using System;

namespace instavote.contracts.adapters
{
    public interface IFeedbackDialog
    {
        void Show(string trainingMatchcode);
        void Retry(string trainingMatchcode, string email, int score, string suggestions, string error);
        void Ack(string trainingMatchcode);

        event Action<string> OnShowRequest;
        event Action<string, string, int, string> OnFeedbackRequest;
    }
}
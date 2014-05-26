using System;

namespace instavote.contracts.adapters
{
    public interface IRegistrationDialog
    {
        void Show(string trainingMatchcode);
        void Retry(string trainingMatchcode, string name, string email, string error);
        void Ack(string trainingMatchcode, string name, string email);

        event Action<string, string, string> OnRegistrationRequest;
        event Action<string> OnShowRequest;
    }
}
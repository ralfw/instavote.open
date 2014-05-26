using System;

namespace instavote.contracts.adapters
{
    public interface IDownloadDialog
    {
        void Deliver(string trainingMatchcode, string csv);
        event Action<string> OnDownloadRequest;
    }
}
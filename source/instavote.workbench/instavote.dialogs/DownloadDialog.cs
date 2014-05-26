using System;
using System.IO;
using Nancy;
using instavote.contracts.adapters;

namespace instavote.dialogs
{
    public class DownloadDialog : IDownloadDialog
    {
        private readonly NancyPortal _portal;

        public DownloadDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/download/{TrainingMatchcode}"] = _ => { 
                OnDownloadRequest(_.TrainingMatchcode);
                return _portal.SharedResponse;
            };
        }


        public void Deliver(string trainingMatchcode, string csv)
        {
            var response = new Response();

            response.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}.csv", trainingMatchcode));
            response.ContentType = "text/csv";
            response.Contents = stream => {
                using (var sw = new StreamWriter(stream)) {
                    sw.Write(csv);
                }
            };

            _portal.SharedResponse = response;
        }


        public event Action<string> OnDownloadRequest;
    }
}

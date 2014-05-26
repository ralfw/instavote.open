using System;
using System.Configuration;
using Nancy;
using instavote.contracts.adapters;
using instavote.dialogs;
using instavote.integration;
using instavote.repository;

namespace instavote.application
{
    public class Program : NancyPortal
    {
        private readonly MongoDbRepository _repo;

        public Program()
        {
            var regDlg = new RegistrationDialog(this);
            var fbDlg = new FeedbackDialog(this);
            var trainingsübersichtDlg = new TrainingsübersichtDialog(this);
            var trainingDlg = new TrainingDialog(this);
            var downloadDlg = new DownloadDialog(this);
            var rootDlg = new RootDialog(this);
            _repo = new MongoDbRepository(ConfigurationManager.AppSettings["MongoDbConnectionstring"]);

            new Application(regDlg, fbDlg, trainingsübersichtDlg, trainingDlg, downloadDlg, _repo);

            Get["/hello"] = _ => "Hello from instavote.application, " + DateTime.Now;
        }
    }
}
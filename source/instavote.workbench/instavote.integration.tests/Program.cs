using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using instavote.contracts.adapters;
using instavote.dialogs;
using instavote.repository;

namespace instavote.integration.tests
{
    public class Program : NancyPortal
    {
        private const string CONNECTIONSTRING = "mongodb://localhost";
        private readonly MongoDbRepository _repo;

        public Program()
        {
            var regDlg = new RegistrationDialog(this);
            var fbDlg = new FeedbackDialog(this);
            var trainingsübersichtDlg = new TrainingsübersichtDialog(this);
            var trainingDlg = new TrainingDialog(this);
            var downloadDlg = new DownloadDialog(this);          
            var rootDlg = new RootDialog(this);
            _repo = new MongoDbRepository(CONNECTIONSTRING);

            new Application(regDlg, fbDlg, trainingsübersichtDlg, trainingDlg, downloadDlg, _repo);

            Get["/reset"] = _ => { ResetDb("xxx"); return this.SharedResponse; };

            Get["/hello"] = _ => "Hello from instavote.integration.tests, " + DateTime.Now;
        }

        public void ResetDb(string trainingMatchcode)
        {
            var cli = new MongoClient(CONNECTIONSTRING);
            var ser = cli.GetServer();
            var db = ser.GetDatabase("Instavote");
            var col = db.GetCollection("Trainings");
            col.RemoveAll();

            _repo.CreateTraining(trainingMatchcode, "trainerFor" + trainingMatchcode,
                _ => this.SharedResponse = string.Format("Created training with matchcode {0} on {1}", trainingMatchcode, CONNECTIONSTRING),
                ex => { throw ex; });
        }
    }
}
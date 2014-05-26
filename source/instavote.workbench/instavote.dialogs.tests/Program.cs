using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using instavote.contracts;
using instavote.contracts.adapters;
using instavote.contracts.adapters.data;

namespace instavote.dialogs.tests
{
    public class Program : NancyPortal
    {
        public Program()
        {
            var regDlg = new RegistrationDialog(this);
            var fbDlg = new FeedbackDialog(this);
            var tüDlg = new TrainingsübersichtDialog(this);
            var trDlg = new TrainingDialog(this);
            var rDlg = new RootDialog(this);

            regDlg.OnShowRequest += regDlg.Show;
            regDlg.OnRegistrationRequest += (tid, name, email) => {
                if (name == "x")
                    regDlg.Retry("someMatchcode", name, email, string.Format("Received: {0}, {1}, {2}", tid, name, email));
                else
                    regDlg.Ack("someMatchcode", name, email);
            };

            fbDlg.OnShowRequest += fbDlg.Show;
            fbDlg.OnFeedbackRequest += (tmatchcode, email, score, suggestions) =>
            {
                if (email == "x")
                    fbDlg.Retry(tmatchcode, email, score, suggestions, string.Format("Received: {0}, {1}, {2}, {3}", tmatchcode, email, score, suggestions));
                else
                    fbDlg.Ack(tmatchcode);
            };

            tüDlg.OnShowRequest += trainerMatchcode => tüDlg.Show(trainerMatchcode,
                new[] { new Tuple<string, string>("1", "training1"), new Tuple<string, string>("2", "training2") }, 
                "");
            tüDlg.OnTrainingCreationRequest += (trainerMatchcode, newTrainingMatchcode) => tüDlg.Show(trainerMatchcode, new[] { new Tuple<string, string>("1", "training1"), new Tuple<string, string>("2", "training2"), new Tuple<string, string>("3", newTrainingMatchcode), },
                                                                                                                    newTrainingMatchcode == "x" ? "Training matchcode already existent." : "");

            trDlg.OnShowRequest += trainingId =>
                {
                    var ti = new TrainingInfo {
                        Matchcode = "someTraining",
                        Votes = new[]
                            {
                                new VoteInfo {Name = "n1", Email = "e1@", Score = 6, Suggestions = "s1"},
                                new VoteInfo {Name = "n2", Email = "e2@", Score = 7, Suggestions = "s2"},
                            },
                        AverageScore = 6.5
                    };
                    trDlg.Show(ti);
                };

            Get["/hello"] = _ => "Hello from instavote.dialogs.tests, " + DateTime.Now;
        }
    }
}
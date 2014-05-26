using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instavote.contracts.adapters;
using instavote.contracts.domaindata;
using instavote.domain;

namespace instavote.integration
{
    public class Application
    {
        public Application(IRegistrationDialog regDlg, 
                           IFeedbackDialog fbDlg, 
                           ITrainingsübersichtDialog trainingsübersichtDlg, 
                           ITrainingDialog trainingDlg,
                           IDownloadDialog downloadDlg,
                           IRepository repo)
        {
            var ia = new Interactors(repo, new Trainings());

            regDlg.OnShowRequest += trainingMatchcode => 
                repo.LoadTrainingByMatchcode(trainingMatchcode,
                    training => regDlg.Show(training.Matchcode),
                    ex => { throw ex; });
            regDlg.OnRegistrationRequest += (trainingMatchcode, name, email) =>
                ia.HandleRegistrationRequest(trainingMatchcode, name, email,
                    () => regDlg.Ack(trainingMatchcode, name, email),
                    err => regDlg.Retry(trainingMatchcode, name, email, err));


            fbDlg.OnShowRequest += trainingMatchcode =>
                repo.LoadTrainingByMatchcode(trainingMatchcode,
                    training => fbDlg.Show(training.Matchcode),
                    ex => { throw ex; });
            fbDlg.OnFeedbackRequest += (trainingMatchcode, email, score, suggestions) =>
                ia.HandleFeedbackRequest(trainingMatchcode, email, score, suggestions,
                    () => fbDlg.Ack(trainingMatchcode),
                    err => fbDlg.Retry(trainingMatchcode, email, score, suggestions, err));


            trainingsübersichtDlg.OnShowRequest += trainerMatchcode => {
                var trainingMatchcodes = ia.HandleShowTrainingsRequest(trainerMatchcode);
                trainingsübersichtDlg.Show(trainerMatchcode, trainingMatchcodes, "");
            };

            trainingsübersichtDlg.OnTrainingCreationRequest += (trainerMatchcode, newTrainingsMatchcode) => 
                ia.HandleTrainingCreationRequest(trainerMatchcode, newTrainingsMatchcode,
                    trainingMatchcodes => trainingsübersichtDlg.Show(trainerMatchcode, trainingMatchcodes, ""),
                    err => {
                        var trainingMatchcodes = ia.HandleShowTrainingsRequest(trainerMatchcode);
                        trainingsübersichtDlg.Show(trainerMatchcode, trainingMatchcodes, err);
                    });

            trainingDlg.OnShowRequest += trainingId => {
                var trainingInfo = ia.HandleTrainingShowRequest(trainingId);
                trainingDlg.Show(trainingInfo);
            };


            downloadDlg.OnDownloadRequest += trainingMatchcode => {
                var csv = ia.HandleDownloadRequest(trainingMatchcode);
                downloadDlg.Deliver(trainingMatchcode, csv);
            };
        }
    }
}

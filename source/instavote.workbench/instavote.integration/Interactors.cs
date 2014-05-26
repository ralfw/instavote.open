using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instavote.contracts.adapters;
using instavote.contracts.adapters.data;
using instavote.contracts.domaindata;
using instavote.domain;
using instavote.repository;

namespace instavote.integration
{
    public class Interactors
    {
        private readonly IRepository _repo;
        private readonly Trainings _trainings;

        public Interactors(IRepository repo, Trainings trainings)
        {
            _repo = repo;
            _trainings = trainings;
        }


        public void HandleRegistrationRequest(string trainingMatchcode, string name, string email, Action onSuccess, Action<string> onError)
        {
            _repo.LoadTrainingByMatchcode(trainingMatchcode,
                training => _trainings.Register_attendee(training, name, email,
                    () => _repo.UpdateTraining(training, onSuccess, ex => onError(ex.Message)),
                    onError),
                ex => onError(ex.Message));
        }


        public void HandleFeedbackRequest(string trainingMatchcode, string email, int score, string suggestions, Action onSuccess, Action<string> onError)
        {
            _repo.LoadTrainingByMatchcode(trainingMatchcode,
                training => _trainings.Note_feedback(training, email, score, suggestions,
                    () => _repo.UpdateTraining(training, onSuccess, ex => onError(ex.Message)),
                    onError),
                ex => onError(ex.Message));
        }


        public Tuple<string,string>[] HandleShowTrainingsRequest(string trainerMatchcode)
        {
            Training[] trainings = _repo.LoadTrainings(trainerMatchcode);
            return trainings.Select(t => new Tuple<string,string>(t.Id.ToString(), t.Matchcode)).ToArray();
        }


        public void HandleTrainingCreationRequest(string trainerMatchcode, string newTrainingMatchcode, Action<Tuple<string,string>[]> onSuccess, Action<string> onError)
        {
            _trainings.Validate_Matchcode(newTrainingMatchcode,
                () => _repo.CreateTraining(newTrainingMatchcode, trainerMatchcode, training =>  {
                            var trainingMatchcodes = HandleShowTrainingsRequest(trainerMatchcode);
                            onSuccess(trainingMatchcodes);
                        },
                        ex => onError(string.Format("Cannot create training '{0}'! Matchcode already taken.", newTrainingMatchcode))),
                onError);
        }

        public TrainingInfo HandleTrainingShowRequest(string trainingId)
        {
            TrainingInfo info = null;
            _repo.LoadTrainingById(trainingId,
                training => {
                    var averageScore = _trainings.Average_scores(training);
                    info = training.ToTrainingInfo(averageScore);
                },
                ex => { throw ex; });
            return info;
        }


        public string HandleDownloadRequest(string trainingMatchcode)
        {
            var csv = "";
            _repo.LoadTrainingByMatchcode(trainingMatchcode, 
                training => csv = training.ToCsv(),
                ex => csv = "Cannot load training data for " + trainingMatchcode + "! Error: " + ex.Message);
            return csv;
        }
    }
}

using System.Linq;
using System.Text;
using instavote.contracts.adapters.data;
using instavote.contracts.domaindata;

namespace instavote.integration
{
    static class Mapper
    {
        public static TrainingInfo ToTrainingInfo(this Training training, double averageScore)
        {
            return new TrainingInfo {
                Id = training.Id.ToString(),
                Matchcode = training.Matchcode,
                AverageScore = averageScore,
                Votes = training.Attendees.Select(a => new VoteInfo{
                    Name = a.Name,
                    Email = a.Email,
                    Score = a.Feedback.Score,
                    Suggestions = a.Feedback.Suggestions
                }).ToArray()
            };
        }


        public static string ToCsv(this Training training)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Training;Trainer;AttendeeName;AttendeeEmail;Score;Suggestions");
            foreach (var a in training.Attendees)
            {
                csv.AppendLine(string.Format("{0};{1};{2};{3};{4};\"{5}\"",
                    training.Matchcode,
                    training.TrainerMatchcode,
                    a.Name,
                    a.Email,
                    a.Feedback.Score,
                    a.Feedback.Suggestions));
            }
            return csv.ToString();
        }
    }
}
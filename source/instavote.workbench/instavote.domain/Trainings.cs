using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using instavote.contracts.domaindata;

namespace instavote.domain
{
    public class Trainings
    {
        public void Register_attendee(Training training, string name, string email, Action onSuccess, Action<string> onError)
        {
            Attendee a;
            if (training.TryGetAttendeeByEmail(email, out a))
                onError(string.Format("Email '{0}' has already been registered.", email));
            else
            {
                training.RegisterAttendee(name, email);
                onSuccess();
            }
        }


        public void Note_feedback(Training training, string email, int score, string suggestions, Action onSuccess, Action<string> onError)
        {
            Attendee a;
            if (training.TryGetAttendeeByEmail(email, out a))
            {
                a.Feedback.Score = score;
                a.Feedback.Suggestions = suggestions;
                onSuccess();
            }
            else
                onError(string.Format("Attendee with email '{0}' hasn´t been registered for training '{1}'.", email, training.Matchcode));
        }


        public double Average_scores(Training training)
        {
            var scores = training.Attendees.Where(a => a.Feedback.Score > 0)
                                           .Select(a => a.Feedback.Score).ToArray();
            return scores.Length > 0 ? scores.Average() : 0.0;
        }


        public void Validate_Matchcode(string newTrainingMatchcode, Action onSuccess, Action<string> onError)
        {
            var encodedMatchcode = HttpUtility.UrlEncode(newTrainingMatchcode);
            if (encodedMatchcode == newTrainingMatchcode)
                onSuccess();
            else
                onError("Matchcode cannot be used in URL. Please choose a different one. Avoid whitespace and special chars.");
        }
    }
}

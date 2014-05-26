using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instavote.contracts.domaindata
{
    public class Training
    {
        public string Id;
        public string Matchcode;
        public string TrainerMatchcode;
        public Attendee[] Attendees = new Attendee[0];

        public void RegisterAttendee(string name, string email)
        {
            if (Attendees.Any(a => a.Email == email)) throw new InvalidOperationException("Attendee with same email address already registered! " + email);
            Attendees = Attendees.Concat(new[] {new Attendee {Name = name, Email = email}}).ToArray();
        }

        public bool TryGetAttendeeByEmail(string email, out Attendee attendee)
        {
            attendee = Attendees.FirstOrDefault(a => a.Email == email);
            return attendee != null;
        }
    }

    public class Attendee
    {
        public string Name;
        public string Email;
        public Feedback Feedback = new Feedback();
    }

    public class Feedback
    {
        public int Score;
        public string Suggestions = "";
    }
}

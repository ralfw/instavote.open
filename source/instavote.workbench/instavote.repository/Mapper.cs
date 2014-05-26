using System;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using instavote.contracts.domaindata;

namespace instavote.repository
{
    static class Mapper
    {
        public static BsonDocument ToBsonDocument(this Training training)
        {
            return new BsonDocument {
                {"_id", string.IsNullOrEmpty(training.Id) ? Guid.NewGuid().ToString() 
                                                          : training.Id},
                {"Matchcode", training.Matchcode},
                {"TrainerMatchcode", training.TrainerMatchcode},
                {
                    "Attendees", new BsonArray(training.Attendees.Select(a => new BsonDocument
                        {
                            {"Name", a.Name},
                            {"Email", a.Email},
                            {
                                "Feedback",
                                new BsonDocument {{"Score", a.Feedback.Score}, {"Suggestions", a.Feedback.Suggestions}}
                            }
                        }))
                }
            };
        }


        public static Training ToTraining(this BsonDocument doc)
        {
            var training = new Training
                {
                    Id = doc["_id"].AsString,
                    Matchcode = doc["Matchcode"].AsString,
                    TrainerMatchcode = doc["TrainerMatchcode"].AsString,
                    Attendees = doc["Attendees"].AsBsonArray
                                                .Select(a =>
                                                        new Attendee
                                                            {
                                                                Name = a["Name"].AsString,
                                                                Email = a["Email"].AsString,
                                                                Feedback = new Feedback
                                                                    {
                                                                        Score = a["Feedback"]["Score"].AsInt32,
                                                                        Suggestions = a["Feedback"]["Suggestions"].AsString,
                                                                    }
                                                            }
                        )                       .ToArray()
                };
            return training;
        }
    }
}
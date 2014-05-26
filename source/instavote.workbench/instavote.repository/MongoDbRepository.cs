using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using instavote.contracts.adapters;
using instavote.contracts.domaindata;

using MongoDB.Bson;
using MongoDB.Driver;

namespace instavote.repository
{
    public class MongoDbRepository : IRepository
    {
        private const string DBNAME = "Instavote";

        private readonly MongoClient _mdbClient;
        private readonly MongoServer _mdbServer;
        private readonly MongoDatabase _mdb;
        private readonly MongoCollection _mdbCol;
            
        public MongoDbRepository(string connectionstring)
        {
            Trace.TraceInformation("MongoDbRepository.ctor, connectionstring: {0}", connectionstring);

            _mdbClient = new MongoClient(connectionstring);
            _mdbServer = _mdbClient.GetServer();
            _mdb = _mdbServer.GetDatabase(DBNAME);

            _mdbCol = _mdb.GetCollection("Trainings");
            _mdbCol.CreateIndex(new IndexKeysBuilder().Ascending("Matchcode"), IndexOptions.SetUnique(true));
            _mdbCol.CreateIndex(new IndexKeysBuilder().Ascending("TrainerMatchcode"));
        }


        public void LoadTrainingByMatchcode(string matchcode, Action<Training> onSucces, Action<Exception> onFailure)
        {
            var doc = _mdbCol.FindOneAs<BsonDocument>(Query.EQ("Matchcode", matchcode));
            if (doc == null) { onFailure(new InvalidOperationException("No training found with this matchcode: " + matchcode)); return; }

            var training = doc.ToTraining();
            onSucces(training);
        }

        public void LoadTrainingById(string id, Action<Training> onSucces, Action<Exception> onFailure)
        {
            var doc = _mdbCol.FindOneAs<BsonDocument>(Query.EQ("_id", id));
            if (doc == null) { onFailure(new InvalidOperationException("No training found with this id: " + id)); return; }

            var training = doc.ToTraining();
            onSucces(training);
        }


        public Training[] LoadTrainings(string trainerMatchcode)
        {
            var docs = _mdbCol.FindAs<BsonDocument>(Query.EQ("TrainerMatchcode", trainerMatchcode));
            return docs.Select(doc => doc.ToTraining()).ToArray();
        }


        public void UpdateTraining(Training training, Action onSucces, Action<Exception> onFailure)
        {
            if (training.Id == null) throw new InvalidOperationException("Cannot update training which has not been created! Missing id. Matchcode: " + training.Matchcode);

            try
            {
                var doc = training.ToBsonDocument();
                var result = _mdbCol.Update(Query.EQ("_id", training.Id), 
                               Update.Replace(doc), 
                               UpdateFlags.None);
                if (result.UpdatedExisting)
                    onSucces();
                else
                    throw new InvalidOperationException("Could not find training to update! Matchcode: " + training.Matchcode);
            }
            catch (Exception ex)
            {
                onFailure(ex);
            }
        }


        public void CreateTraining(string matchcode, string trainerMatchcode, Action<Training> onSuccess, Action<Exception> onFailure)
        {
            var training = new Training {Matchcode = matchcode, TrainerMatchcode = trainerMatchcode};
            var doc = training.ToBsonDocument();
            try
            {
                _mdbCol.Insert(doc);
                training.Id = doc["_id"].AsString;
                onSuccess(training);
            }
            catch (Exception ex)
            {
                onFailure(ex);
            }
        }
    }
}

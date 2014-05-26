using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Driver;
using equalidator;
using instavote.contracts.adapters;
using instavote.contracts.domaindata;

namespace instavote.repository.tests
{
    [TestFixture]
    public class test_MongoDbRepository
    {
        private const string CONNECTIONSTRING = "mongodb://localhost";
        private MongoCollection _col;


        [SetUp]
        public void ResetDb()
        {
            var cli = new MongoClient(CONNECTIONSTRING);
            var ser = cli.GetServer();
            var db = ser.GetDatabase("Instavote");
            _col = db.GetCollection("Trainings");
            _col.RemoveAll();
        }


        [Test, Explicit]
        public void Create_training()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training result = null;
            sut.CreateTraining("sometraining", "sometrainer", _ => result = _, null);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual("sometraining", result.Matchcode);
            Assert.AreEqual("sometrainer", result.TrainerMatchcode);
        }


        [Test, Explicit]
        public void Load_training_by_matchcode()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training t = null;
            sut.CreateTraining("sometrainingLoad", "sometrainerLoad", _ => t = _, null);

            Training result = null;
            sut.LoadTrainingByMatchcode("sometrainingLoad", _ => result = _, null);

            Equalidator.AreEqual(t.Id, result.Id);
            Assert.AreEqual("sometrainerLoad", result.TrainerMatchcode);
        }

        [Test, Explicit]
        public void Load_non_existing_training()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Exception ex = null;
            sut.LoadTrainingByMatchcode("xyz-nonexisting", null, _ => ex = _);

            Assert.IsNotNull(ex);
            Console.WriteLine("Exception thrown as expected: {0}", ex.Message);
        }


        [Test, Explicit]
        public void Load_training_by_id()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training t = null;
            sut.CreateTraining("sometrainingLoad", "sometrainerLoad", _ => t = _, null);

            Training result = null;
            sut.LoadTrainingByMatchcode(t.Id, _ => result = _, null);

            Equalidator.AreEqual(t.Id, result.Id);
            Assert.AreEqual("sometrainerLoad", result.TrainerMatchcode);
        }

        [Test, Explicit]
        public void Load_training_by_id_via_string()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training t = null;
            sut.CreateTraining("sometrainingLoad", "sometrainerLoad", _ => t = _, null);

            Console.WriteLine("id: {0}", t.Id);

            Training result = null;
            sut.LoadTrainingById(t.Id, _ => result = _, null);

            Equalidator.AreEqual(t.Id, result.Id);
            Assert.AreEqual("sometrainerLoad", result.TrainerMatchcode);
        }


        [Test, Explicit]
        public void Update_training()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training t = null;
            sut.CreateTraining("sometrainingUpdate", "sometrainerUpdate", _ => t = _, null);

            t.TrainerMatchcode = "new trainer matchcode";
            sut.UpdateTraining(t, () => Console.WriteLine("Updated training: " + t.Matchcode), null);

            Training result = null;
            sut.LoadTrainingByMatchcode("sometrainingUpdate", _ => result = _, null);
            Assert.AreEqual("new trainer matchcode", result.TrainerMatchcode);
        }


        [Test, Explicit]
        public void Trying_to_update_non_existent_training_fails()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            Training t = null;
            sut.CreateTraining("sometrainingUpdate2", "sometrainerUpdate2", _ => t = _, null);
            _col.RemoveAll();

            t.TrainerMatchcode = "this change wont make it";

            Exception ex = null;
            sut.UpdateTraining(t, null, _ => ex = _);

            Assert.IsInstanceOf<InvalidOperationException>(ex);
            Console.WriteLine("Exception thrown as expected: {0}", ex.Message);

        }


        [Test, Explicit]
        public void Load_trainings()
        {
            IRepository sut = new MongoDbRepository(CONNECTIONSTRING);

            var trainings = sut.LoadTrainings("sometrainer");
            Assert.AreEqual(0, trainings.Length);

            sut.CreateTraining("abc", "sometrainer", _ => { }, ex => { throw ex; });
            sut.CreateTraining("xyz", "sometrainer", _ => { }, ex => { throw ex; });

            var matchcodes = sut.LoadTrainings("sometrainer").Select(t => t.Matchcode).ToArray();
            Assert.That(matchcodes, Is.EquivalentTo(new[] { "abc", "xyz" }));
        }
    }
}

using System;
using MongoDB.Driver;
using NUnit.Framework;

namespace instavote.repository.tests
{
    [TestFixture]
    public class test_MongoHQ
    {
        [Test]
        public void Check_connection()
        {
            const string mongoHqConnectionstring = "...your connection string here...";
            var cli = new MongoClient(mongoHqConnectionstring);
            var ser = cli.GetServer();
            var db = ser.GetDatabase("Instavote");
            var col = db.GetCollection("Trainings");

            var stats = col.GetStats();
            Console.WriteLine("Stat: {0}", stats.ObjectCount);

            new MongoDbRepository(mongoHqConnectionstring);
        } 
    }
}
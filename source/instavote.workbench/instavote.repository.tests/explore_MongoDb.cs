using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace instavote.repository.tests
{
    [TestFixture]
    public class explore_MongoDb
    {
        private const string CONNECTIONSTRING = "mongodb://localhost";

        [Test, Explicit]
        public void Experiment_with_id_type()
        {
            var cli = new MongoClient(CONNECTIONSTRING);
            var ser = cli.GetServer();
            var db = ser.GetDatabase("Instavote");
            var col = db.GetCollection("Testdata");

            var doc = new BsonDocument();
            var guid = Guid.NewGuid().ToString();
            doc.Add(new BsonElement("_id", guid));
            doc.Add(new BsonElement("text", "hello " + DateTime.Now));

            col.Insert(doc);

            var found = col.FindOne(Query.EQ("_id", guid.ToString()));
            Assert.AreEqual(doc["text"], found["text"]);
        }
    }
}

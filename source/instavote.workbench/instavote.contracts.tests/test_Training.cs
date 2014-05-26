using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using equalidator;
using instavote.contracts.domaindata;

namespace instavote.contracts.tests
{
    [TestFixture]
    public class test_Training
    {
        [Test]
        public void Adding_an_attendee()
        {
            var sut = new Training();

            sut.RegisterAttendee("a", "ea");

            Equalidator.AreEqual(new[]{new Attendee{Name = "a", Email = "ea"}}, sut.Attendees);

            Assert.Throws<InvalidOperationException>(() => sut.RegisterAttendee("a", "ea"));
        }


        [Test]
        public void Retrieving_an_attendee()
        {
            var sut = new Training();

            sut.RegisterAttendee("a", "ea");

            Attendee a;
            Assert.IsTrue(sut.TryGetAttendeeByEmail("ea", out a));
            Assert.AreEqual("a", a.Name);

            Assert.IsFalse(sut.TryGetAttendeeByEmail("x", out a));
        }
    }
}

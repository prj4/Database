using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.Azure
{
    class EventRepositoryTest
    {
        private string _connectionString =
            "Server=tcp:katrinesphotobook.database.windows.net,1433;Initial Catalog=PhotoBook4;" +
            "Persist Security Info=False;User ID=Ingeniørhøjskolen@katrinesphotobook.database.windows.net;" +
            "Password=Katrinebjergvej22;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private IEventRepository _uut;

        public EventRepositoryTest()
        {
           
        }

        #region Sources
        private static Event[] EventSource =
        {
            new Event{Location = "Lokation4", Description = "Beskrivelse4", Name = "Event4", HostId = 1, Pin = 4, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation5", Description = "Beskrivelse5", Name = "Event5", HostId = 2, Pin = 5, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation6", Description = "Beskrivelse6", Name = "Event6", HostId = 3, Pin = 6, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new EventRepository(_connectionString);
        }

        [TearDown]
        public void TearDown()
        { }
        #endregion

        #region Success Tests
        [TestCase("Event4")]
        [TestCase("Event5")]
        [TestCase("Event6")]
        public void GetEvents_GettingListOfEventsAndFindingSpecific_ReturnsTrue(string name)
        {
            IQueryable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e.Name == name);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void InsertEvent_InsertEventAndFind_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);

            IQueryable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e.Pin == eve.Pin);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void GetEventByPin_AddFindCompare_ReturnsTrue(Event eve)
        {
            if (_uut.GetEvent(eve.Pin) != null)
                _uut.InsertEvent(eve);

            var result = _uut.GetEvent(eve.Pin).Result;

            Assert.AreEqual(eve.Pin, result.Pin);
        }

        [Test, TestCaseSource("EventSource")]
        public void GetEventByName_AddFindCompare_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);
            var result = _uut.GetEvent(eve.Name).Result;

            Assert.AreEqual(eve.Pin, result.Pin);
        }

        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByPin_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve);

            _uut.DeleteEvent(eve.Pin);

            IQueryable<Event> result = _uut.GetEvents().Result;

            Assert.AreEqual(null, result);
        }

        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByName_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve);

            _uut.DeleteEvent(eve.Name);

            IQueryable<Event> result = _uut.GetEvents().Result;

            Assert.AreEqual(null, result);
        }

        [Test, TestCaseSource("EventSource")]
        public void UpdateEvent_InsertChangeDescriptionCheck_EqualsNewDescription(Event eve)
        {
            _uut.InsertEvent(eve);

            var tempEve = _uut.GetEvent(eve.Pin).Result;

            tempEve.Description = "NewDescription1";

            _uut.UpdateEvent(tempEve);

            var result = _uut.GetEvent(eve.Pin).Result;

            Assert.AreEqual("NewDescription1", result.Description);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetEventById_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEvent(1).Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetEventByName_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEvent("Test").Result;

            Assert.AreEqual(null, result);
        }
        #endregion

    }
}
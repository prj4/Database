using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.InMemory
{
    class EventRepositoryTest
    {
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;
        private IEventRepository _uut;

        public EventRepositoryTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        #region Sources
        private static Event[] EventSource =
        {
            new Event{Location = "Lokation4", Description = "Beskrivelse4", Name = "Event4", HostId = 1, Pin = "9876", StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation5", Description = "Beskrivelse5", Name = "Event5", HostId = 2, Pin = "7865", StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation6", Description = "Beskrivelse6", Name = "Event6", HostId = 3, Pin = "5634", StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new EventRepository(_InMemoryOptions);
        }

        [TearDown]
        public void TearDown()
        { }
        #endregion

        #region Success Tests
        [Test, TestCaseSource("EventSource")]
        public void GetEvents_GettingListOfEventsAndFindingSpecific_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);

            IEnumerable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e.Name == eve.Name);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void InsertEvent_InsertEventAndFind_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);

            IEnumerable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e.Pin == eve.Pin);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void GetEventByPin_AddFindCompare_ReturnsTrue(Event eve)
        {
            if (_uut.GetEventByPin(eve.Pin) != null)
                _uut.InsertEvent(eve);

            var result = _uut.GetEventByPin(eve.Pin).Result;

            Assert.AreEqual(eve.Pin, result.Pin);
        }

        
        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByPin_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve);

            _uut.DeleteEventByPin(eve.Pin);

            IEnumerable<Event> result = _uut.GetEvents().Result;

            Assert.AreEqual(null, result);
        }


        [Test, TestCaseSource("EventSource")]
        public void UpdateEvent_InsertChangeDescriptionCheck_EqualsNewDescription(Event eve)
        {
            _uut.InsertEvent(eve);

            var tempEve = _uut.GetEventByPin(eve.Pin).Result;

            tempEve.Description = "NewDescription1";

            _uut.UpdateEvent(tempEve);

            var result = _uut.GetEventByPin(eve.Pin).Result;

            Assert.AreEqual("NewDescription1", result.Description);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetEventById_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEventByPin("1").Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetEventByName_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEventByPin("Test").Result;

            Assert.AreEqual(null, result);
        }

        public void GetEvent_GettingEventsAttachedToHost_ReturnsTrue()
        {
            var Event1 = new Event
            {
                Location = "Lokation", Description = "Beskrivelse", Name = "Event", HostId = 5, Pin = "4351",
                StartDate = DateTime.Now, EndDate = DateTime.MaxValue
            };
            var Event2 = new Event
            {
                Location = "Lokation", Description = "Beskrivelse", Name = "Event", HostId = 5, Pin = "3451",
                StartDate = DateTime.Now, EndDate = DateTime.MaxValue
            };
            _uut.InsertEvent(Event1);
            _uut.InsertEvent(Event2);

            var events = _uut.GetEventsByHostId(Event1.HostId).Result;



            var result = events.Count(e => e.HostId == Event1.HostId);

            Assert.AreEqual(null, result);
        }


        #endregion

    }
}
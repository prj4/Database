using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PhotoBook.Repository.EventRepository;
using PhotoBook.Repository.HostRepository;
using PhotoBook.Test;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
{
    class EventRepositoryTest
    {
        private InMemoryDatabaseHelper _inMemoryDatabaseHelper;
        private IEventRepository _uut;

        #region Sources
        private static Event[] EventSource =
        {
            new Event{Location = "Lokation1", Description = "Beskrivelse1", Name = "Event1", HostId = 1, Pin = 1, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation2", Description = "Beskrivelse2", Name = "Event2", HostId = 2, Pin = 2, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
            new Event{Location = "Lokation3", Description = "Beskrivelse3", Name = "Event3", HostId = 3, Pin = 3, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _inMemoryDatabaseHelper = new InMemoryDatabaseHelper("UnitTest");
            _uut = new EventRepository(new PhotoBookDbContext(_inMemoryDatabaseHelper._options));

            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                context.Database.EnsureDeleted();
            }
        }
        #endregion

        #region Success Tests
        [TestCase("Event1")]
        [TestCase("Event2")]
        [TestCase("Event3")]
        public void GetEvents_GettingListOfEventsAndFindingSpecific_ReturnsTrue(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();
            }


            IQueryable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e.Name == name);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void InsertEvent_InsertEventAndFind_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);

            IQueryable<Event> events = _uut.GetEvents().Result;

            bool result = events.Any(e => e == eve);

            Assert.True(result);
        }

        [Test, TestCaseSource("EventSource")]
        public void GetEventByPin_AddFindCompare_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);
            var result = _uut.GetEvent(eve.Pin).Result;

            Assert.AreEqual(eve, result);
        }

        [Test, TestCaseSource("EventSource")]
        public void GetEventByName_AddFindCompare_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve);
            var result = _uut.GetEvent(eve.Name).Result;

            Assert.AreEqual(eve, result);
        }

        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByPin_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve);

            _uut.DeleteEvent(eve.Pin);

            IQueryable<Event> result = _uut.GetEvents().Result;

            Assert.AreEqual(null,result);
        }

        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByName_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve);

            _uut.DeleteEvent(eve.Name);

            IQueryable<Event> result = _uut.GetEvents().Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateEvent_InsertChangeDescriptionCheck_EqualsNewDescription()
        {
            var eveBefore = new Event
            {
                Location = "Lokation1",
                Description = "Description1",
                Name = "Event1",
                HostId = 1,
                Pin = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue
            };

            var eveAfter = new Event
            {
                Location = "Lokation1",
                Description = "NewDescription1",
                Name = "Event1",
                HostId = 1,
                Pin = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue
            };

            _uut.InsertEvent(eveBefore);

            _uut.UpdateEvent(eveAfter);

            var result = _uut.GetEvent(eveBefore.Pin).Result;

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
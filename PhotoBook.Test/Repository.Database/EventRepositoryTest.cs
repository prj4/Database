using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventRepository;
using PhotoBook.Repository.PictureRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

/*EN UNIT TEST AF GANGEN*/
namespace PhotoBook.Test.Repository.Database
{
    class EventRepositoryTest
    {

        private IEventRepository _uut;

        public EventRepositoryTest()
        {


        }

        #region Sources

        private static Event[] EventSource =
        {
            new Event
            {
                Location = "Lokation4", Description = "Beskrivelse4", Name = "Event4", HostId = 1, Pin = "9876",
                StartDate = DateTime.Now, EndDate = DateTime.MaxValue
            },
            new Event
            {
                Location = "Lokation5", Description = "Beskrivelse5", Name = "Event5", HostId = 2, Pin = "7865",
                StartDate = DateTime.Now, EndDate = DateTime.MaxValue
            },
            new Event
            {
                Location = "Lokation6", Description = "Beskrivelse6", Name = "Event6", HostId = 3, Pin = "5634",
                StartDate = DateTime.Now, EndDate = DateTime.MaxValue
            },
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new EventRepository(new PhotoBookDbContext());
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        #region Success Tests

        [Test, TestCaseSource("EventSource")]
        public void GetEvents_GettingListOfEventsAndFindingSpecific_ReturnsTrue(Event eve)
        {
            _uut.InsertEvent(eve).Wait();

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

            _uut.InsertEvent(eve).Wait();

            var result = _uut.GetEventByPin(eve.Pin).Result;

            Assert.AreEqual(eve.Pin, result.Pin);
        }


        [Test, TestCaseSource("EventSource")]
        public void DeleteEventByPin_InserteDeleteCheckIfNothing_EqualsNull(Event eve)
        {
            _uut.InsertEvent(eve).Wait();

            _uut.DeleteEventByPin(eve.Pin).Wait();

            var result = _uut.GetEventByPin(eve.Pin).Result;

            Assert.AreEqual(null, result);
        }


        [Test, TestCaseSource("EventSource")]
        public void UpdateEvent_InsertChangeDescriptionCheck_EqualsNewDescription(Event eve)
        {
            _uut.InsertEvent(eve).Wait();

            var tempEve = _uut.GetEventByPin(eve.Pin).Result;

            tempEve.Description = "NewDescription1";

            _uut.UpdateEvent(tempEve).Wait();

            var result = _uut.GetEventByPin(eve.Pin).Result;

            Assert.AreEqual("NewDescription1", result.Description);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetEventById_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEventByPin("9999").Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetEventByName_TryingToGetNonExistingEvent_ReturnsNull()
        {
            var result = _uut.GetEventByPin("Test").Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetEventById_TryingToGetNonExistingPicture_ReturnsNull()
        {
            
            var host = new Host {Email = "Email1@email.com", Name = "Host"};

            using (var context = new PhotoBookDbContext())
            {
                context.Hosts.Add(host);
            }

            
            var eve = new Event
            {
                Location = "Lokation4",
                Description = "Beskrivelse4",
                Name = "Event4",
                HostId = host.HostId,
                Pin = "7656",
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue
            };

            _uut.InsertEvent(eve).Wait();
            _uut.DeleteEventByPin(eve.Pin);

            var pic = new Picture { EventPin = "7656", HostId = host.HostId };

            using (var context = new PhotoBookDbContext())
            {
                context.Pictures.Add(pic);
                var result = context.Pictures.FirstOrDefaultAsync(p => p.PictureId == p.PictureId).Result;

                Assert.AreEqual(null, result);
            }

            #endregion

        }
    }
}
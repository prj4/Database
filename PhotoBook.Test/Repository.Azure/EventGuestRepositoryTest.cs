using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventGuestRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.Azure
{
    class EventGuestRepositoryTest
    {
        private IEventGuestRepository _uut;

        private string _connectionString =
            "Server=tcp:katrinesphotobook.database.windows.net,1433;Initial Catalog=PhotoBook4;" +
            "Persist Security Info=False;User ID=Ingeniørhøjskolen@katrinesphotobook.database.windows.net;" +
            "Password=Katrinebjergvej22;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public EventGuestRepositoryTest()
        {
        }
        #region Sources
        private static EventGuest[] EventGuestSource =
        {
            new EventGuest {Event_Pin = 3, Guest_Id = 1},
            new EventGuest {Event_Pin = 3, Guest_Id = 2},
            new EventGuest {Event_Pin = 3, Guest_Id = 3}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            
            _uut = new EventGuestRepository(_connectionString);

            var eventGuests = _uut.GetEventGuests().Result;

            if (eventGuests != null)
                foreach (var eg in eventGuests)
                {
                    _uut.DeleteEventGuest(eg);
                }
        }

        [TearDown]
        public void TearDown()
        {
            var eventGuests = _uut.GetEventGuests().Result;

            if (eventGuests != null)
                foreach (var eg in eventGuests)
                {
                    _uut.DeleteEventGuest(eg);
                }
        }
        #endregion

        #region Success Tests

        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuests_InsertingCheckingIfIQueryableContains_ReturnsTrue(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuests().Result;

            bool result = eventGuests
                .Any(eg => (eg.Event_Pin == eventGuest.Event_Pin) &&
                           (eg.Guest_Id == eventGuest.Guest_Id));

            Assert.True(result);
        }


        [Test, TestCaseSource("EventGuestSource")]
        public void InsertEventGuest_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {

            _uut.InsertEventGuest(eventGuest);
            IQueryable<EventGuest> eventGuests = _uut.GetEventGuests().Result;

            bool result = eventGuests
                .Any(eg => (eg.Event_Pin == eventGuest.Event_Pin) &&
                           (eg.Guest_Id == eventGuest.Guest_Id));

            Assert.True(result);
        }
        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuestByEventPin_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuestsByEventPin(eventGuest.Event_Pin).Result;

            bool result = eventGuests
                .Any(eg => (eg.Event_Pin == eventGuest.Event_Pin) &&
                           (eg.Guest_Id == eventGuest.Guest_Id));

            Assert.True(result);
        }

        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuestByGuestId_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {
            
                _uut.InsertEventGuest(eventGuest);

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuestsByGuestId(eventGuest.Guest_Id).Result;

            bool result = eventGuests
                .Any(eg => (eg.Event_Pin == eventGuest.Event_Pin) &&
                           (eg.Guest_Id == eventGuest.Guest_Id));

            Assert.True(result);
        }

        [Test, TestCaseSource("EventGuestSource")]
        public void DeleteEventguest_InsertDeleteFind_ReturnsNull(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);

            _uut.DeleteEventGuest(eventGuest);

            var result = _uut.GetEventGuestsByEventPin(eventGuest.Event_Pin).Result;

           Assert.AreEqual(null, result);
        }
        #endregion

        #region Failure/Corner Tests

        #endregion
    }
}

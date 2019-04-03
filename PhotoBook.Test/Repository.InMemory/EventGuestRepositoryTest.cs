using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventGuestRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.InMemory
{
    class EventGuestRepositoryTest
    {
        private IEventGuestRepository _uut;
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;

        public EventGuestRepositoryTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
        #region Sources
        private static EventGuest[] EventGuestSource =
        {
            new EventGuest {EventPin = 3, GuestId = 1},
            new EventGuest {EventPin = 3, GuestId = 2},
            new EventGuest {EventPin = 3, GuestId = 3}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new EventGuestRepository(_InMemoryOptions);
        }

        [TearDown]
        public void TearDown()
        {
        }
        #endregion

        #region Success Tests

        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuests_InsertingCheckingIfIQueryableContains_ReturnsTrue(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);
            IQueryable<EventGuest> eventGuests = _uut.GetEventGuests().Result;

            bool result = eventGuests
                .Any(eg => (eg.EventPin == eventGuest.EventPin) &&
                           (eg.GuestId == eventGuest.GuestId));

            

            Assert.True(result);
        }


        [Test, TestCaseSource("EventGuestSource")]
        public void InsertEventGuest_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {

            _uut.InsertEventGuest(eventGuest);
            IQueryable<EventGuest> eventGuests = _uut.GetEventGuests().Result;

            bool result = eventGuests
                .Any(eg => (eg.EventPin == eventGuest.EventPin) &&
                           (eg.GuestId == eventGuest.GuestId));

            

            Assert.True(result);
        }
        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuestByEventPin_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuestsByEventPin(eventGuest.EventPin).Result;

            bool result = eventGuests
                .Any(eg => (eg.EventPin == eventGuest.EventPin) &&
                           (eg.GuestId == eventGuest.GuestId));

            

            Assert.True(result);
        }

        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuestByGuestId_InsertAndFind_returnsTrue(EventGuest eventGuest)
        {
            
                _uut.InsertEventGuest(eventGuest);

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuestsByGuestId(eventGuest.GuestId).Result;

            bool result = eventGuests
                .Any(eg => (eg.EventPin == eventGuest.EventPin) &&
                           (eg.GuestId == eventGuest.GuestId));


            Assert.True(result);
        }

        [Test, TestCaseSource("EventGuestSource")]
        public void DeleteEventguest_InsertDeleteFind_ReturnsNull(EventGuest eventGuest)
        {
            _uut.InsertEventGuest(eventGuest);

            _uut.DeleteEventGuest(eventGuest);

            var result = _uut.GetEventGuestsByEventPin(eventGuest.EventPin).Result;

           Assert.AreEqual(null, result);
        }
        #endregion

        #region Failure/Corner Tests

        #endregion
    }
}

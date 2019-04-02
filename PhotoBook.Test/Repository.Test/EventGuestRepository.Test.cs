using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.EventGuestRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
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
            new EventGuest {Event_Pin = 3, Guest_Id = 1},
            new EventGuest {Event_Pin = 3, Guest_Id = 2},
            new EventGuest {Event_Pin = 3, Guest_Id = 3}
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

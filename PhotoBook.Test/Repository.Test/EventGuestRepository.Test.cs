using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PhotoBook.Repository.EventGuestRepository;
using PhotoBook.Repository.EventRepository;
using PhotoBook.Test;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
{
    class EventGuestRepositoryTest
    {
        private InMemoryDatabaseHelper _inMemoryDatabaseHelper;
        private IEventGuestRepository _uut;

        #region Sources
        private static EventGuest[] EventGuestSource =
        {
            new EventGuest {Event_Pin = 1, Guest_Id = 4},
            new EventGuest {Event_Pin = 2, Guest_Id = 5},
            new EventGuest {Event_Pin = 3, Guest_Id = 6}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _inMemoryDatabaseHelper = new InMemoryDatabaseHelper("UnitTest");
            _uut = new EventGuestRepository(new PhotoBookDbContext(_inMemoryDatabaseHelper._options));

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

        [TestCase(1, 4)]
        [TestCase(2, 5)]
        [TestCase(3, 6)]
        public void GetEventGuests_CheckingIfIQueryableContains_ReturnsTrue(int eventPin, int guestId)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();
            }

            IQueryable<EventGuest> eventGuests = _uut.GetEventGuests().Result;

            bool result = eventGuests
                .Any(eg => (eg.Event_Pin == eventPin) && 
                           (eg.Guest_Id == guestId));

            Assert.True(result);
        }

        //TODO: Missing test of the rest of EventGuest 

        [Test, TestCaseSource("EventGuestSource")]
        public void GetEventGuestByEventPin(EventGuest eventGuest)
        {

        }

        #endregion

        #region Failure/Corner Tests

        #endregion
    }
}

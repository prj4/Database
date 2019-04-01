using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PhotoBook.Repository.EventRepository;
using PhotoBook.Repository.GuestRepository;
using PhotoBook.Test;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
{
    class GuestRepositoryTest
    {
        private InMemoryDatabaseHelper _inMemoryDatabaseHelper;
        private IGuestRepository _uut;

        #region Sources
        private static Guest[] GuestSource =
        {
            new Guest{Name = "Guest1"},
            new Guest{Name = "Guest2"},
            new Guest{Name = "Guest3"}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _inMemoryDatabaseHelper = new InMemoryDatabaseHelper("UnitTest");
            _uut = new GuestRepository(new PhotoBookDbContext(_inMemoryDatabaseHelper._options));

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
        [TestCase("Guest1")]
        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void GetGuests_GettingListOfGuestsAndFindingSpecific_ReturnsTrue(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();
            }


            IQueryable<Guest> guests = _uut.GetGuests().Result;

            bool result = guests.Any(e => e.Name == name);

            Assert.True(result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void InsertGuest_InsertGuestAndFind_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);

            IQueryable<Guest> guests = _uut.GetGuests().Result;

            bool result = guests.Any(g => g == guest);

            Assert.True(result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestById_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);
            var result = _uut.GetGuest(guest.PictureTakerId).Result;

            Assert.AreEqual(guest, result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestByName_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);
            var result = _uut.GetGuest(guest.Name).Result;

            Assert.AreEqual(guest, result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void DeleteGuestByPin_InserteDeleteCheckIfNothing_EqualsNull(Guest guest)
        {
            _uut.InsertGuest(guest);

            _uut.DeleteGuest(guest.PictureTakerId);

            IQueryable<Guest> result = _uut.GetGuests().Result;

            Assert.AreEqual(null, result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void DeleteGuestByName_InserteDeleteCheckIfNothing_EqualsNull(Guest guest)
        {
            _uut.InsertGuest(guest);

            _uut.DeleteGuest(guest.Name);

            IQueryable<Guest> result = _uut.GetGuests().Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateGuest_InsertChangeNameCheck_EqualsNewDescription()
        {
            var GuestBefore = new Guest{PictureTakerId = 1, Name = "Guest1"};

            var GuestAfter = new Guest {PictureTakerId = 1, Name = "NewGuest1" };

            _uut.InsertGuest(GuestBefore);

            _uut.UpdateGuest(GuestAfter);

            var result = _uut.GetGuest(1).Result;

            Assert.AreEqual("NewGuest1", result.Name);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetGuestById_TryingToGetNonExistingGuest_ReturnsNull()
        {
            var result = _uut.GetGuest(1).Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetGuestByName_TryingToGetNonExistingGuest_ReturnsNull()
        {
            var result = _uut.GetGuest("Test").Result;

            Assert.AreEqual(null, result);
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.GuestRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.InMemory
{
    class GuestRepositoryTest
    {
        private IGuestRepository _uut;
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;

        public GuestRepositoryTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        #region Sources
        private static Guest[] GuestSource =
        {
            new Guest{Name = "Guest1",EventPin = "1"},
            new Guest{Name = "Guest2", EventPin = "1"},
            new Guest{Name = "Guest3", EventPin = "1"}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new GuestRepository(new PhotoBookDbContext(_InMemoryOptions));
        }

        [TearDown]
        public void TearDown()
        {
        }
        #endregion

        #region Success Tests
        [TestCase("Guest1")]
        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void GetGuests_GettingListOfGuestsAndFindingSpecific_ReturnsTrue(string name)
        {
            IEnumerable<Guest> guests = _uut.GetGuests().Result;

            bool result = guests.Any(e => e.Name == name);

            Assert.True(result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void InsertGuest_InsertGuestAndFind_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);

            IEnumerable<Guest> guests = _uut.GetGuests().Result;

            bool result = guests.Any(g => g.GuestId == guest.GuestId);

            Assert.True(result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestById_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);
            var result = _uut.GetGuestById(guest.GuestId).Result;

            Assert.AreEqual(guest.GuestId, result.GuestId);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestByNameAndEventPin_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest);
            var result = _uut.GetGuestByNameAndEventPin(guest.Name, "1").Result;

            Assert.AreEqual(guest.GuestId, result.GuestId);
        }

        [Test, TestCaseSource("GuestSource")]
        public void DeleteGuestByPin_InserteDeleteCheckIfNothing_EqualsNull(Guest guest)
        {
            _uut.InsertGuest(guest);

            _uut.DeleteGuestById(guest.GuestId);

            IEnumerable<Guest> result = _uut.GetGuests().Result;

            Assert.AreEqual(null, result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void DeleteGuestByName_InserteDeleteCheckIfNothing_EqualsNull(Guest guest)
        {
            _uut.InsertGuest(guest);

            _uut.DeleteGuestByNameAndEventPin(guest.Name, "1");

            IEnumerable<Guest> result = _uut.GetGuests().Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateGuest_InsertChangeNameCheck_EqualsNewDescription()
        {
            var GuestBefore = new Guest{GuestId = 1, Name = "Guest1"};

            var GuestAfter = new Guest {GuestId = 1, Name = "NewGuest1" };

            _uut.InsertGuest(GuestBefore);

            _uut.UpdateGuest(GuestAfter);

            var result = _uut.GetGuestById(1).Result;

            Assert.AreEqual("NewGuest1", result.Name);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetGuestById_TryingToGetNonExistingGuest_ReturnsNull()
        {
            var result = _uut.GetGuestById(99).Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetGuestByNameAndEventPin_TryingToGetNonExistingGuest_ReturnsNull()
        {
            var result = _uut.GetGuestByNameAndEventPin("Test","100").Result;

            Assert.AreEqual(null, result);
        }
        #endregion

    }
}

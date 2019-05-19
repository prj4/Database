using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.GuestRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

/*Need to have a seeded Event with pin "1" to run*/
/*EN UNIT TEST AF GANGEN*/
namespace PhotoBook.Test.Repository.Database
{
    class GuestRepositoryTest
    {
        private IGuestRepository _uut;
        

        public GuestRepositoryTest()
        {
            
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
            _uut = new GuestRepository(new PhotoBookDbContext());
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
            _uut.InsertGuest(guest).Wait();

            IEnumerable<Guest> guests = _uut.GetGuests().Result;

            bool result = guests.Any(g => g.GuestId == guest.GuestId);

            Assert.True(result);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestById_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest).Wait();
            var result = _uut.GetGuestById(guest.GuestId).Result;

            Assert.AreEqual(guest.GuestId, result.GuestId);
        }

        [Test, TestCaseSource("GuestSource")]
        public void GetGuestByNameAndEventPin_AddFindCompare_ReturnsTrue(Guest guest)
        {
            _uut.InsertGuest(guest).Wait();
            var result = _uut.GetGuestByNameAndEventPin(guest.Name, guest.EventPin).Result;

            Assert.AreEqual(guest.Name, result.Name);
        }

        [Test, TestCaseSource("GuestSource")]
        public void DeleteGuestByPin_InserteDeleteCheckIfNothing_EqualsNull(Guest guest)
        {
            _uut.InsertGuest(guest).Wait();

            _uut.DeleteGuestById(guest.GuestId).Wait();

            var result = _uut.GetGuestById(guest.GuestId).Result;

            Assert.AreEqual(null, result);
        }

        

        [Test]
        public void UpdateGuest_InsertChangeNameCheck_EqualsNewDescription()
        {
            var GuestBefore = new Guest{ Name = "Guest1", EventPin = "1"};

            

            _uut.InsertGuest(GuestBefore).Wait();

            var GuestAfter = new Guest {GuestId = GuestBefore.GuestId, Name = "NewGuest1", EventPin = "1" };

            _uut.UpdateGuest(GuestAfter).Wait();

            var result = _uut.GetGuestById(GuestBefore.GuestId).Result;

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

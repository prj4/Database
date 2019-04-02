using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Database.Test
{
    public class DatabaseTest
    {
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;
        private PhotoBookDbContext _context;

        public DatabaseTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase("Test")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
        #region Setup and TearDown   
        [SetUp]
        public void Setup()
        {
            _context = new PhotoBookDbContext(_InMemoryOptions);

            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
        #endregion

        #region Creation/Read Tests
        
        [TestCase("Username1")]
        [TestCase("Username2")]
        [TestCase("Username3")]
        public void CreationOfHost_SearchingOnUserName_ReturnsTrue(string userName)
        {
           

                var result = _context.Hosts
                    .Where(x => x.Username == userName)
                    .FirstOrDefault().Username;

                Assert.AreEqual(result,userName);
        }



        [TestCase("Guest1")]
        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void CreationOfGuest_SearchingOnName_ReturnsTrue(string name)
        {
            var result = _context.Guests
                .Where(g => g.Name == name)
                .FirstOrDefault().Name;

            Assert.AreEqual(result, name);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void CreationOfEvent_SearchingOnPin_ReturnsTrue(int pin)
        {
            var result = _context.Events
                .Where(e => e.Pin == pin)
                .FirstOrDefault().Pin;

            Assert.AreEqual(pin, result);
        }

        [TestCase("wwwroot/Images/1.png")]
        [TestCase("wwwroot/Images/2.png")]
        [TestCase("wwwroot/Images/3.png")]
        public void CreationOfPicture_SearchingOnPUrl_ReturnsTrue(string url)
        {
                var result = _context.Pictures
                    .Where(p => p.URL == url)
                    .FirstOrDefault().URL;

                Assert.AreEqual(url, result);
            
        }

        [TestCase(1,4)]
        [TestCase(2,5)]
        [TestCase(3,6)]
        public void CreationOfEventGuest_SearchingOnEventPin_ReturnsTrueOnGuestId(int eventPin, int guestId)
        {
            var result = _context.EventGuests
                .Where(eg => eg.Event_Pin == eventPin)
                .FirstOrDefault().Guest_Id;

            Assert.AreEqual(guestId, result);

        }






        #endregion

        #region Update/Read Tests
        [TestCase("Email1@email.com","MortenRosenquist@gmail.com")]
        [TestCase("Email2@email.com","MortenLyng@gmail.com")]
        [TestCase("Email3@email.com", "Morten@gmail.com")]
        public void UpdateOfHost_SearchingOnEmailAndChanging_ReturnsTrue(string emailBefore, string emailAfter)
        {
            var result = _context.Hosts
                .Where(x => x.Email == emailBefore)
                .FirstOrDefault().Email = emailAfter;

            Assert.AreEqual(emailAfter, result);
        }



        [TestCase("Guest1", "NewGuest1")]
        [TestCase("Guest2", "NewGuest2")]
        [TestCase("Guest3", "NewGuest3")]
        public void UpdateOfGuest_SearchingOnNameAndChanging_ReturnsTrue(string nameBefore, string nameAfter)
        {
            var result = _context.Guests
                .Where(g => g.Name == nameBefore)
                .FirstOrDefault().Name = nameAfter;

            Assert.AreEqual(nameAfter, result);
        }


        [TestCase("Beskrivelse1", "NewDescription1")]
        [TestCase("Beskrivelse2", "NewDescription2")]
        [TestCase("Beskrivelse3", "NewDescription3")]
        public void UpdateOfEvent_SearchingOnDescriptionAndChanging_ReturnsTrue(string descriptionBefore, string descriptionAfter)
        {
            var result = _context.Events
                .Where(e => e.Description == descriptionBefore)
                .FirstOrDefault().Description = descriptionAfter;

            Assert.AreEqual(descriptionAfter, result);
        }

        [TestCase("wwwroot/Images/1.png", "New/URL1")]
        [TestCase("wwwroot/Images/2.png", "New/URL2")]
        [TestCase("wwwroot/Images/3.png", "New/URL3")]
        public void UpdateOfPicture_SearchingOnPUrlAndChanging_ReturnsTrue(string urlBefore, string urlAfter)
        {
            var result = _context.Pictures
                .Where(p => p.URL == urlBefore)
                .FirstOrDefault().URL = urlAfter;

            Assert.AreEqual(urlAfter, result);
        }
#endregion

        #region Deletion/Read Tests
        
        [TestCase("Host2")]
        [TestCase("Host3")]
        public void DeletionOfHost_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            var host = _context.Hosts.FirstOrDefault(h => h.Name == name);
            _context.Hosts.Remove(host);
            _context.SaveChanges();

            bool result = _context.Hosts.Any(h => h.Name == name);
            Assert.False(result);
        }

        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void DeletionOfGuest_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            var Guest = _context.Guests.FirstOrDefault(h => h.Name == name);
            _context.Guests.Remove(Guest);
            _context.SaveChanges();

            bool result = _context.Guests.Any(h => h.Name == name);
            Assert.False(result);
        }

        
        [TestCase("Event3")]
        public void DeletionOfEvent_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            var Event = _context.Events.FirstOrDefault(h => h.Name == name);

            _context.Events.Remove(Event);
            _context.SaveChanges();

            bool result = _context.Events.Any(h => h.Name == name);
            Assert.False(result);
        }

        [TestCase("wwwroot/Images/1.png")]
        [TestCase("wwwroot/Images/2.png")]
        [TestCase("wwwroot/Images/3.png")]
        public void DeletionOPicture_SearchingOnURLDeletingAndFinding_returnsFalse(string url)
        {
                var picture = _context.Pictures.FirstOrDefault(p => p.URL == url);
                _context.Pictures.Remove(picture);
                _context.SaveChanges();

                bool result = _context.Pictures.Any(p => p.URL == url);
                Assert.False(result);
            
        }



        #endregion

        #region Corner Cases

        

        #endregion







    }
}
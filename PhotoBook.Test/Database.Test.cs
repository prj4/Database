using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PhotoBook.Test;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Database.Test
{
    public class Tests
    {
        #region TestHelpers
        
        private InMemoryDatabaseHelper _inMemoryDatabase;
        #endregion

        #region Setup and TearDown   
        [SetUp]
        public void Setup()
        {
            _inMemoryDatabase = new InMemoryDatabaseHelper("UnitTest");

            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                context.Database.EnsureDeleted();
            }
        }
        #endregion

        #region Creation/Read Tests
        
        [TestCase("Username1")]
        [TestCase("Username2")]
        [TestCase("Username3")]
        public void CreationOfHost_SearchingOnUserName_ReturnsTrue(string userName)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();
                

                var result = context.Hosts
                    .Where(x => x.Username == userName)
                    .FirstOrDefault().Username;

                Assert.AreEqual(result,userName);
            }
        }



        [TestCase("Guest1")]
        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void CreationOfGuest_SearchingOnName_ReturnsTrue(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Guests
                    .Where(g => g.Name == name)
                    .FirstOrDefault().Name;

                Assert.AreEqual(result, name);
            }
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void CreationOfEvent_SearchingOnPin_ReturnsTrue(int pin)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Events
                    .Where(e => e.Pin == pin)
                    .FirstOrDefault().Pin;

                Assert.AreEqual(pin, result);
            }
        }

        [TestCase("wwwroot/Images/1.png")]
        [TestCase("wwwroot/Images/2.png")]
        [TestCase("wwwroot/Images/3.png")]
        public void CreationOfPicture_SearchingOnPUrl_ReturnsTrue(string url)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Pictures
                    .Where(p => p.URL == url)
                    .FirstOrDefault().URL;

                Assert.AreEqual(url, result);
            }
        }

        [TestCase(1,2)]
        [TestCase(2,3)]
        [TestCase(3,1)]
        public void CreationOfEventGuest_SearchingOnEventPin_ReturnsTrueOnGuestId(int eventPin, int guestId)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.EventGuests
                    .Where(eg => eg.Event_Pin == eventPin)
                    .FirstOrDefault().Guest_Id;

                Assert.AreEqual(guestId, result);
            }
        }






        #endregion

        #region Update/Read Tests
        [TestCase("Email1@email.com","MortenRosenquist@gmail.com")]
        [TestCase("Email2@email.com","MortenLyng@gmail.com")]
        [TestCase("Email3@email.com", "Morten@gmail.com")]
        public void UpdateOfHost_SearchingOnEmailAndChanging_ReturnsTrue(string emailBefore, string emailAfter)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Hosts
                    .Where(x => x.Email == emailBefore)
                    .FirstOrDefault().Email = emailAfter;

                Assert.AreEqual(emailAfter, result);
            }
        }



        [TestCase("Guest1", "NewGuest1")]
        [TestCase("Guest2", "NewGuest2")]
        [TestCase("Guest3", "NewGuest3")]
        public void UpdateOfGuest_SearchingOnNameAndChanging_ReturnsTrue(string nameBefore, string nameAfter)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Guests
                    .Where(g => g.Name == nameBefore)
                    .FirstOrDefault().Name = nameAfter;

                Assert.AreEqual(nameAfter, result);
            }
        }


        [TestCase("Beskrivelse1", "NewDescription1")]
        [TestCase("Beskrivelse2", "NewDescription2")]
        [TestCase("Beskrivelse3", "NewDescription3")]
        public void UpdateOfEvent_SearchingOnDescriptionAndChanging_ReturnsTrue(string descriptionBefore, string descriptionAfter)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Events
                    .Where(e => e.Description == descriptionBefore)
                    .FirstOrDefault().Description = descriptionAfter;

                Assert.AreEqual(descriptionAfter, result);
            }
        }

        [TestCase("wwwroot/Images/1.png", "New/URL1")]
        [TestCase("wwwroot/Images/2.png", "New/URL2")]
        [TestCase("wwwroot/Images/3.png", "New/URL3")]
        public void UpdateOfPicture_SearchingOnPUrlAndChanging_ReturnsTrue(string urlBefore, string urlAfter)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var result = context.Pictures
                    .Where(p => p.URL == urlBefore)
                    .FirstOrDefault().URL = urlAfter;

                Assert.AreEqual(urlAfter, result);
            }
        }
#endregion

        #region Deletion/Read Tests
        
        [TestCase("Host2")]
        [TestCase("Host3")]
        public void DeletionOfHost_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var host = context.Hosts.FirstOrDefault(h => h.Name == name);
                context.Hosts.Remove(host);
                context.SaveChanges();

                bool result = context.Hosts.Any(h => h.Name == name);
                Assert.False(result);
            }
        }

        [TestCase("Guest2")]
        [TestCase("Guest3")]
        public void DeletionOfGuest_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var Guest = context.Guests.FirstOrDefault(h => h.Name == name);
                context.Guests.Remove(Guest);
                context.SaveChanges();

                bool result = context.Guests.Any(h => h.Name == name);
                Assert.False(result);
            }
        }

        
        [TestCase("Event3")]
        public void DeletionOfEvent_SearchingOnNameDeletingAndFinding_returnsFalse(string name)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var Event = context.Events.FirstOrDefault(h => h.Name == name);
                context.Events.Remove(Event);
                context.SaveChanges();

                bool result = context.Events.Any(h => h.Name == name);
                Assert.False(result);
            }
        }

        [TestCase("wwwroot/Images/1.png")]
        [TestCase("wwwroot/Images/2.png")]
        [TestCase("wwwroot/Images/3.png")]
        public void DeletionOPicture_SearchingOnURLDeletingAndFinding_returnsFalse(string url)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();

                var picture = context.Pictures.FirstOrDefault(p => p.URL == url);
                context.Pictures.Remove(picture);
                context.SaveChanges();

                bool result = context.Pictures.Any(p => p.URL == url);
                Assert.False(result);
            }
        }



        #endregion

        #region Corner Cases

        

        #endregion







    }
}
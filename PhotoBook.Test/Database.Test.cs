using System.Collections.Generic;
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
        internal class DataSeeder
        {
            private DbContext _context;
            public DataSeeder(DbContext context)
            {
                _context = context;
            }

            public void SeedData()
            {
                _context.AddRange( new List<Host>()
                    {
                        new Host{Email = "Email1@email.com", Name = "Host1", Username = "Username1", PW = "PWPWPWPWPW1"},
                        new Host{Email = "Email2@email.com", Name = "Host2", Username = "Username2", PW = "PWPWPWPWPW2"},
                        new Host{Email = "Email3@email.com", Name = "Host3", Username = "Username3", PW = "PWPWPWPWPW3"},
                    }
                    );

                _context.AddRange(new List<Guest>()
                    {
                        new Guest{Name = "Guest1"},
                        new Guest{Name = "Guest2"},
                        new Guest{Name = "Guest3"}
                    }
                );

                _context.AddRange(new List<Event>()
                    {
                        new Event{Location = "Lokation1", Description = "Beskrivelse1", Name = "Event1", HostId = 1, Pin = 1},
                        new Event{Location = "Lokation2", Description = "Beskrivelse2", Name = "Event2", HostId = 2, Pin = 2},
                        new Event{Location = "Lokation3", Description = "Beskrivelse3", Name = "Event3", HostId = 3, Pin = 3},
                    }
                );

                _context.AddRange(new List<Picture>()
                {
                    new Picture {EventPin = 1, Taker = 1, URL = "wwwroot/Images/1.png"},
                    new Picture {EventPin = 2, Taker = 2, URL = "wwwroot/Images/2.png"},
                    new Picture {EventPin = 3, Taker = 3, URL = "wwwroot/Images/3.png"}
                    }
                );

                _context.AddRange(new List<EventGuest>()
                    {
                        new EventGuest {Event_Pin = 1, Guest_Id = 2},
                        new EventGuest {Event_Pin = 2, Guest_Id = 3},
                        new EventGuest {Event_Pin = 3, Guest_Id = 1}
                    }
                );
                
                
                

                _context.SaveChanges();
            }
        }
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

        #region Creation Tests
        
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







    }
}
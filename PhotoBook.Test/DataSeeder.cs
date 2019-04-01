using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test
{
    internal class DataSeeder
    {
        private DbContext _context;
        public DataSeeder(DbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            _context.AddRange(new List<Host>()
                    {
                        new Host{Email = "Email1@email.com", Name = "Host", Username = "Username1", PW = "PWPWPWPWPW1"},
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
                        new Event{Location = "Lokation1", Description = "Beskrivelse1", Name = "Event1", HostId = 1, Pin = 1, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
                        new Event{Location = "Lokation2", Description = "Beskrivelse2", Name = "Event2", HostId = 2, Pin = 2, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
                        new Event{Location = "Lokation3", Description = "Beskrivelse3", Name = "Event3", HostId = 3, Pin = 3, StartDate = DateTime.Now, EndDate = DateTime.MaxValue},
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
}

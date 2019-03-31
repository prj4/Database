using System;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Model;

namespace PhotoBookDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands.CreateHost(new Host
            {
                Email = "LyngRosenquistMorten@gmail.com",
                Username = "Zorozero999",
                PW = "morten:)",
                Name = "Morten Rosenquist",
            });
            

            Commands.CreateEvent(new Event
            {
                Pin = 1234,
                Location = "Aarhus",
                Description = "Fødselsdag",
                Name = "Mortens fødselsdag",
                StartDate = DateTime.Now,
                EndDate = DateTime.Today,
                HostName = "Morten Rosenquist"
            });

            Commands.CreateGuests(new Guest
            {
                Name = "Troels Bleicken"    
            });

            Commands.CreateEventGuests(new EventGuests
            {
                EventPin = 1234,
                GuestName = "Troels Bleicken",
            });
            

            Commands.CreatePictures(new Picture
            {
                EventPin = 1234,
                PictureId = 1,
                Taker = "Troels Bleicken",
                URL = "FedtURL/EndnuFeder",
            });

            Commands.ListAll();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBookDatabase
{
    public class Commands
    {
        #region TestData Creation

        public static async void CreateHost(Host host)
        {
            
            using (var db = new PhotoBookDbContext())
            {
                try
                {
                   db.Hosts.Add(host);
                    
                    if (await db.SaveChangesAsync() > 0)
                    {
                        Console.WriteLine("New Host added");
                    }
                    else
                    {
                        Console.WriteLine("No new Host added");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
                
               
            }
        }

        public static async void CreateEvent(Event eve)
        {
            using (var db = new PhotoBookDbContext())
            {
                try
                {
                    db.Events.Add(eve);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        Console.WriteLine("New Event added");
                    }
                    else
                    {
                        Console.WriteLine("No new Event added");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        

        public static async void CreateGuests(Guest guest)
        {
            using (var db = new PhotoBookDbContext())
            {
                try
                {
                    db.Guests.Add(guest);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        Console.WriteLine("New guest added");
                    }
                    else
                    {
                        Console.WriteLine("No new guest added");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        public static async void CreateEventGuests(EventGuests eventGuests)
        {
            using (var db = new PhotoBookDbContext())
            {
                try
                {
                    db.EventsGuests.Add(eventGuests);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        Console.WriteLine("New EventGuest added");
                    }
                    else
                    {
                        Console.WriteLine("No new EventGuest added");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        public static async void CreatePictures(Picture picture)
        {
            using (var db = new PhotoBookDbContext())
            {
                try
                {
                    db.Pictures.Add(picture);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        Console.WriteLine("New Picture added");
                    }
                    else
                    {
                        Console.WriteLine("No new Picture added");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        




        #endregion

        #region Listing
        public static void ListAll()
        {
            //ListHosts();
            //ListGuests();
            ListEvents();
            
            ListEventGuests();
            
            ListPictures();
            ListPictureTakers();
        }

        public static void ListEvents()
        {
            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("Events:");
                try
                {
                    foreach (var e in db.Events)
                        Console.WriteLine(e.ToString());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                
                
            }
        }
        public static void ListGuests()
        {
            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("Guests:");
                foreach (var g in db.Guests)
                {
                    Console.WriteLine(g.ToString());
                }
            }
        }

        public static void ListEventGuests()
        {
            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("EventGuests:");
                foreach (var e in db.EventsGuests)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }


        public static void ListHosts()
        {

            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("Hosts:");
                foreach (var h in db.Hosts)
                {
                    Console.WriteLine(h.ToString());
                }
            }
        }

        public static void ListPictures()
        {
            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("Pictures:");
                foreach (var p in db.Pictures)
                {
                    Console.WriteLine(p.ToString());
                }
            }
        }

        public static void ListPictureTakers()
        {
            using (var db = new PhotoBookDbContext())
            {
                Console.WriteLine("PictureTakers:");
                foreach (var p in db.PictureTakers)
                {
                    Console.WriteLine(p.ToString());
                }
            }
        }
        #endregion
    }
}

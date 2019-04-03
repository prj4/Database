using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Model;


namespace PhotoBookDatabase.Data
{
    public class PhotoBookDbContext : DbContext
    {
       
        public DbSet<Event> Events { get; set; }
        
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureTaker> PictureTakers { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }

        public PhotoBookDbContext()
        { }
        public PhotoBookDbContext(DbContextOptions<PhotoBookDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:katrinesphotobook.database.windows.net,1433;Initial Catalog=PhotoBook4;Persist Security Info=False;User ID=Ingeniørhøjskolen@katrinesphotobook.database.windows.net;Password=Katrinebjergvej22;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                
            }
            
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            HostOnModelCreating(modelBuilder);
            GuestOnModelCreating(modelBuilder);
            EventOnModelCreating(modelBuilder);           
            PictureOnModelCreating(modelBuilder);
            EventGuestOnModelCreating(modelBuilder);
        }

        private void EventGuestOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventGuest>().HasKey(eg => new {eg.EventPin, eg.GuestId});

            modelBuilder.Entity<EventGuest>()
                .HasOne<Event>(sc => sc.Event)
                .WithMany(s => s.EventGuests)
                .HasForeignKey(sc => sc.EventPin);

            modelBuilder.Entity<EventGuest>()
                .HasOne<Guest>(sc => sc.Guest)
                .WithMany(s => s.EventGuests)
                .HasForeignKey(sc => sc.GuestId);

            modelBuilder.Entity<EventGuest>().HasData(
                new EventGuest { EventPin = 1, GuestId = 4 },
                new EventGuest { EventPin = 2, GuestId = 5 },
                new EventGuest { EventPin = 3, GuestId = 6 }
            );

        }

        void GuestOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasBaseType<PictureTaker>();

            modelBuilder.Entity<Guest>().HasData(
                new Guest { Name = "Guest1", PictureTakerId = 4 },
                new Guest { Name = "Guest2", PictureTakerId = 5 },
                new Guest { Name = "Guest3", PictureTakerId = 6 }
                );

        }

        void EventOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Pictures)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventPin);

            modelBuilder.Entity<Event>().HasData(
                new Event { Location = "Lokation1", Description = "Beskrivelse1", Name = "Event1", HostId = 1, Pin = 1, StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation2", Description = "Beskrivelse2", Name = "Event2", HostId = 2, Pin = 2, StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation3", Description = "Beskrivelse3", Name = "Event3", HostId = 3, Pin = 3, StartDate = DateTime.Now, EndDate = DateTime.MaxValue }
                );
        }

        void HostOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .HasBaseType<PictureTaker>();

            modelBuilder.Entity<Host>()
                .HasMany(h => h.Events)
                .WithOne(e => e.Host)
                .HasForeignKey(e => e.HostId);

            

            modelBuilder.Entity<Host>(h =>
                h.HasIndex(e => e.Email).IsUnique());

            modelBuilder.Entity<Host>()
                .HasData(
                    new Host
                    {
                        Email = "Email1@email.com", Name = "Host1",
                        PictureTakerId = 1
                    },
                    new Host
                    {
                        Email = "Email2@email.com", Name = "Host2",
                        PictureTakerId = 2
                    },
                    new Host
                    {
                        Email = "Email3@email.com", Name = "Host3",
                        PictureTakerId = 3
                    }
                );

        }

        void PictureOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne(p => p.PictureTaker)
                .WithMany(p => p.Pictures)
                .HasForeignKey(p => p.TakerId);

            modelBuilder.Entity<Picture>()
                .HasData(
                    new Picture {PictureId = 1, EventPin = 1, TakerId = 1, URL = "wwwroot/Images/1.png"},
                    new Picture {PictureId = 2, EventPin = 2, TakerId = 2, URL = "wwwroot/Images/2.png"},
                    new Picture {PictureId = 3, EventPin = 3, TakerId = 3, URL = "wwwroot/Images/3.png"}
                );
        }
    }
}

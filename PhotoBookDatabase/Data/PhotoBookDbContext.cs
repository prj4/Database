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
        

        public PhotoBookDbContext()
        { }
        public PhotoBookDbContext(DbContextOptions<PhotoBookDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:katrinesphotobook.database.windows.net,1433;Initial Catalog=PhotoBook5;Persist Security Info=False;User ID=Ingeniørhøjskolen@katrinesphotobook.database.windows.net;Password=Katrinebjergvej22;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");  
            }   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            HostOnModelCreating(modelBuilder);
            EventOnModelCreating(modelBuilder);
            GuestOnModelCreating(modelBuilder);
            PictureOnModelCreating(modelBuilder);
        }

        void GuestOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasBaseType<PictureTaker>();

            modelBuilder.Entity<Guest>()
                .HasOne<Event>(g => g.Event)
                .WithMany(e => e.Guests)
                .HasForeignKey(g => g.EventPin)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Guest>().HasData(
                new Guest { Name = "Guest1", PictureTakerId = 5, EventPin = "1"},
                new Guest { Name = "Guest2", PictureTakerId = 6, EventPin = "2" },
                new Guest { Name = "Guest3", PictureTakerId = 7, EventPin = "3" }
                );

        }

        void EventOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Pictures)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventPin)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>().HasData(
                new Event { Location = "Lokation1", Description = "Beskrivelse1", Name = "Event1", HostId = 1, Pin = "1", StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation2", Description = "Beskrivelse2", Name = "Event2", HostId = 2, Pin = "2", StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation3", Description = "Beskrivelse3", Name = "Event3", HostId = 3, Pin = "3", StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation4", Description = "Beskrivelse4", Name = "Event4", HostId = 1, Pin = "1234", StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation5", Description = "Beskrivelse5", Name = "Event5", HostId = 2, Pin = "2345", StartDate = DateTime.Now, EndDate = DateTime.MaxValue },
                new Event { Location = "Lokation6", Description = "Beskrivelse6", Name = "Event6", HostId = 3, Pin = "3456", StartDate = DateTime.Now, EndDate = DateTime.MaxValue }
                );
        }

        void HostOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .HasBaseType<PictureTaker>();

            modelBuilder.Entity<Host>(h =>
                h.HasIndex(e => e.Email).IsUnique());

            modelBuilder.Entity<Host>()
                .HasMany(h => h.Events)
                .WithOne(h => h.Host)
                .HasForeignKey(e => e.HostId)
                .OnDelete(DeleteBehavior.Restrict);


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
                    },
                    new Host
                    {
                        Email = "Email5@email.com", Name = "Host5",
                        PictureTakerId = 4
                    }
                );

        }

        void PictureOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne<PictureTaker>(p => p.PictureTaker)
                .WithMany(pt => pt.Pictures)
                .HasForeignKey(p => p.TakerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Picture>()
                .HasData(
                    new Picture {PictureId = 1, EventPin = "1", TakerId = 1},
                    new Picture {PictureId = 2, EventPin = "2", TakerId = 2},
                    new Picture {PictureId = 3, EventPin = "3", TakerId = 3}
                );
        }
    }
}

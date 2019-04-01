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
            modelBuilder.Entity<EventGuest>().HasKey(eg => new {eg.Event_Pin, eg.Guest_Id});

            modelBuilder.Entity<EventGuest>()
                .HasOne<Event>(sc => sc.Event)
                .WithMany(s => s.EventGuests)
                .HasForeignKey(sc => sc.Event_Pin).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventGuest>()
                .HasOne<Guest>(sc => sc.Guest)
                .WithMany(s => s.EventGuests)
                .HasForeignKey(sc => sc.Guest_Id).OnDelete(DeleteBehavior.Cascade);
        }

        void GuestOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasBaseType<PictureTaker>();

        }

        void EventOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Pictures)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventPin);
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
                h.HasIndex(u => u.Username).IsUnique());

            modelBuilder.Entity<Host>(h =>
                h.HasIndex(e => e.Email).IsUnique());

        }

        void PictureOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne(p => p.PictureTaker)
                .WithMany(p => p.Pictures)
                .HasForeignKey(p => p.Taker);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;
[assembly: InternalsVisibleTo("PhotoBook.Test")]
namespace PhotoBook.Repository.GuestRepository
{
    public class GuestRepository : IGuestRepository
    {
        private DbContextOptions<PhotoBookDbContext> _options;

        internal GuestRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _options = options;
        }

        public GuestRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Guests.AnyAsync();
                return result;
            }
        }
        private async Task<bool> Exists(Guest guest)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Guests.AnyAsync(g => g.PictureTakerId == guest.PictureTakerId);
                return result;
            }
        }
        private async Task<bool> Exists(int id)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Guests
                    .AnyAsync(g => g.PictureTakerId == id))
                    return true;
                return false;
            }
        }
        private async Task<bool> Exists(string name)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Guests
                    .AnyAsync(g => g.Name == name))
                    return true;
                return false;
            }
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IQueryable<Guest>> GetGuests()
        {
            if (IfAny().Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var guests = await context.Guests.ToListAsync();
                    return guests.AsQueryable();
                }
            }
            return null;
        }

        public async Task<Guest> GetGuest(int id)
        {
            if (Exists(id).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var guest = await context.Guests
                        .FindAsync(id);
                    return guest;
                }  
            }
            return null;
        }

        public async Task<Guest> GetGuest(string name)
        {
            if (Exists(name).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var guest = await context.Guests
                        .Where(x => x.Name == name)
                        .FirstOrDefaultAsync();

                    return guest;
                }
            }
            return null;
        }

        public async void InsertGuest(Guest guest)
        {
            if (!Exists(guest).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    await context.Guests.AddAsync(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteGuest(int id)
        {
            if (Exists(id).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var guest = context.Guests
                        .FindAsync(id).Result;

                    context.Guests.Remove(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteGuest(string name)
        {
            if (Exists(name).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var guest = context.Guests
                        .Where(x => x.Name == name)
                        .FirstOrDefaultAsync().Result;

                    context.Guests.Remove(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void UpdateGuest(Guest guest)
        {
            if (Exists(guest).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var entity = await context.Guests.FindAsync(guest.PictureTakerId);

                    entity.Name = guest.Name;
                    context.Update(entity);
                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion

    }
}

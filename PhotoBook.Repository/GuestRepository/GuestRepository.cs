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
        private PhotoBookDbContext _context;
        private DbContextOptions<PhotoBookDbContext> _options;

        internal GuestRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _context = new PhotoBookDbContext(options);
        }

        public GuestRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new PhotoBookDbContext(_options);
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Guests.AnyAsync();
            return result;
        }
        private async Task<bool> Exists(Guest guest)
        {
            bool result = await _context.Guests.AnyAsync(g => g.PictureTakerId == guest.PictureTakerId);
            return result;
        }
        private async Task<bool> Exists(int id)
        {
            if (await _context.Guests
                .AnyAsync(g => g.PictureTakerId == id))
                return true;
            return false;
        }
        private async Task<bool> Exists(string name)
        {
            if (await _context.Guests
                .AnyAsync(g => g.Name == name))
                return true;
            return false;
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IQueryable<Guest>> GetGuests()
        {
            if (IfAny().Result)
            {
                var guests = await _context.Guests.ToListAsync();
                return guests.AsQueryable();
            }

            return null;
        }

        public async Task<Guest> GetGuest(int id)
        {
            if (Exists(id).Result)
            {
                var guest = await _context.Guests
                    .FindAsync(id);
                return guest;
            }

            return null;
        }

        public async Task<Guest> GetGuest(string name)
        {
            if (Exists(name).Result)
            {
                var guest = await _context.Guests
                    .Where(x => x.Name == name)
                    .FirstOrDefaultAsync();

                return guest;
            }
            return null;
        }

        public async void InsertGuest(Guest guest)
        {
            if (!Exists(guest).Result)
            {
                await _context.Guests.AddAsync(guest);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteGuest(int id)
        {
            if (Exists(id).Result)
            {
                var guest = _context.Guests
                    .FindAsync(id).Result;

                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteGuest(string name)
        {
            if (Exists(name).Result)
            {
                var guest = _context.Guests
                    .Where(x => x.Name == name)
                    .FirstOrDefaultAsync().Result;

                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateGuest(Guest guest)
        {
            if (Exists(guest).Result)
            {
                var entity =  await _context.Guests.FindAsync(guest.PictureTakerId);

                entity.Name = guest.Name;
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

    }
}

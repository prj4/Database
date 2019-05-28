using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.GuestRepository
{
    public class GuestRepository : IGuestRepository
    {
        private PhotoBookDbContext _context;

        public GuestRepository(PhotoBookDbContext context)
        {
            _context = context;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Guests.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByGuest(Guest guest)
        {

            bool result = await _context.Guests.AnyAsync(g => g.GuestId == guest.GuestId);
            return result;
        }

        private async Task<bool> ExistsById(int id)
        {
            if (await _context.Guests
                .AnyAsync(g => g.GuestId == id))
                return true;
            return false;
        }

        private async Task<bool> ExistsByNameAndEventPin(string name, string eventPin)
        {

            if (await _context.Guests
                .AnyAsync(g => (g.Name == name) && (g.EventPin == eventPin)))
                return true;
            return false;
        }

        private async Task<bool> ExistsByName(string name)
        {
            if (await _context.Guests
                .AnyAsync(g => g.Name == name))
                return true;
            return false;
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IEnumerable<Guest>> GetGuests()
        {
            if (IfAny().Result)
            {
                var guests = await _context.Guests.ToListAsync();
                return guests.AsEnumerable();
            }

            return null;
        }

        public async Task<Guest> GetGuestById(int id)
        {
            if (ExistsById(id).Result)
            {
                var guest = await _context.Guests
                    .FindAsync(id);
                return guest;
            }
            return null;
        }


        public async Task<Guest> GetGuestByNameAndEventPin(string name, string eventPin)
        {
            if (ExistsByNameAndEventPin(name, eventPin).Result)
            {
                var guest = await _context.Guests
                    .Where(x => x.Name == name)
                    .FirstOrDefaultAsync();
                return guest;
            }
            return null;
        }

        public async Task InsertGuest(Guest guest)
        {
            if (!ExistsByGuest(guest).Result)
            {
                await _context.Guests.AddAsync(guest);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteGuestById(int id)
        {
            if (ExistsById(id).Result)
            {
                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    var guest = _context.Guests
                        .Include(g => g.Pictures)
                        .FirstOrDefault(g => g.GuestId == id);
                        

                    if (guest.Pictures.Count > 0)
                    {
                        _context.Pictures.RemoveRange(guest.Pictures);
                    }



                    _context.Guests.Remove(guest);
                    transaction.Result.Commit();

                    while (transaction.IsCompleted != true)
                    { }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGuestByNameAndEventPin(string name, string eventPin)
        {
            if (ExistsByNameAndEventPin(name, eventPin).Result)
            {
                var guest = _context.Guests
                    .Where(x => (x.Name == name) &&
                                (x.EventPin == eventPin))
                    .FirstOrDefaultAsync().Result;

                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateGuest(Guest guest)
        {
            if (ExistsByGuest(guest).Result)
            {
                var entity = await _context.Guests.FindAsync(guest.GuestId);

                entity.Name = guest.Name;
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

    }
}

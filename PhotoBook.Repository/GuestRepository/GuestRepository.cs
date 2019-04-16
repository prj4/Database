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

        private async Task<bool> IfAny(PhotoBookDbContext context)
        {
            bool result = await context.Guests.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByGuest(Guest guest, PhotoBookDbContext context)
        {

            bool result = await context.Guests.AnyAsync(g => g.PictureTakerId == guest.PictureTakerId);
            return result;
        }

        private async Task<bool> ExistsById(int id, PhotoBookDbContext context)
        {
            if (await context.Guests
                .AnyAsync(g => g.PictureTakerId == id))
                return true;
            return false;
        }

        private async Task<bool> ExistsByNameAndEventPin(string name, string eventPin, PhotoBookDbContext context)
        {
            
                if (await context.Guests
                    .AnyAsync(g => (g.Name == name) && (g.EventPin == eventPin)))
                    return true;
                return false;
        }
        private async Task<bool> ExistsByName(string name, PhotoBookDbContext context)
        {
            if (await context.Guests
                    .AnyAsync(g => g.Name == name))
                    return true;
                return false;
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IEnumerable<Guest>> GetGuests()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (IfAny(context).Result)
                { 
                    var guests = await context.Guests.ToListAsync();
                    return guests.AsEnumerable();
                }
            }
            return null;
        }

        public async Task<Guest> GetGuestById(int id)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(id, context).Result)
                {
                    var guest = await context.Guests
                        .FindAsync(id);
                    return guest;
                }
            }
            return null;
        }

        
        public async Task<Guest> GetGuestByNameAndEventPin(string name, string eventPin)
        {
                using (var context = new PhotoBookDbContext(_options))
                {
                    if (ExistsByNameAndEventPin(name, eventPin, context).Result)
                    {
                        var guest = await context.Guests
                            .Where(x => x.Name == name)
                            .FirstOrDefaultAsync();
                        return guest;
                    }
                }
            return null;
        }

        public async Task InsertGuest(Guest guest)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (!ExistsByGuest(guest, context).Result)
                {
                    await context.Guests.AddAsync(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteGuestById(int id)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(id, context).Result)
                {
                    var guest = context.Guests
                        .FindAsync(id).Result;

                    context.Guests.Remove(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteGuestByNameAndEventPin(string name, string eventPin)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByNameAndEventPin(name, eventPin, context).Result)
                {
                    var guest = context.Guests
                        .Where(x => (x.Name == name) &&
                                    (x.EventPin == eventPin))
                        .FirstOrDefaultAsync().Result;

                    context.Guests.Remove(guest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateGuest(Guest guest)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByGuest(guest,context).Result)
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

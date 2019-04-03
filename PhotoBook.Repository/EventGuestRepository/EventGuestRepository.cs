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
 
namespace PhotoBook.Repository.EventGuestRepository
{
    public class EventGuestRepository : IEventGuestRepository
    {
        
        private DbContextOptions<PhotoBookDbContext> _options;

        internal EventGuestRepository(DbContextOptions<PhotoBookDbContext> options)
        {

            _options = options;
        }

        public EventGuestRepository(string connectionString)
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
                bool result = await context.EventGuests.AnyAsync();
                return result;
            }
        }
        private async Task<bool> Exists(EventGuest eventGuest)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.EventGuests
                    .AnyAsync(eg => (eg.EventPin == eventGuest.EventPin) &&
                                    (eg.GuestId == eventGuest.GuestId));
                return result;
            }
        }
        private async Task<bool> ExistsByEventPin(int eventPin)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.EventGuests
                    .AnyAsync(eg => eg.EventPin == eventPin))
                    return true;
                return false;
            }
        }
        private async Task<bool> ExistsByGuestId(int guestId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.EventGuests
                    .AnyAsync(eg => eg.GuestId == guestId))
                    return true;
                return false;
            }
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IQueryable<EventGuest>> GetEventGuests()
        {
            if (IfAny().Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var eventGuests = await context.EventGuests.ToListAsync();
                    return eventGuests.AsQueryable();
                }
            }
            return null;
        }

        public async Task<IQueryable<EventGuest>> GetEventGuestsByEventPin(int eventPin)
        {
            if (ExistsByEventPin(eventPin).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var eventGuests = await context.EventGuests.Where(eg => eg.EventPin == eventPin).ToListAsync();

                    return eventGuests.AsQueryable();
                }
            }
            return null;
            
        }

        public async Task<IQueryable<EventGuest>> GetEventGuestsByGuestId(int guestId)
        {
            if (ExistsByGuestId(guestId).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var eventGuests = await context.EventGuests.Where(eg => eg.GuestId == guestId).ToListAsync();

                    return eventGuests.AsQueryable();
                }
            }
            return null;
            
        }

        public async void InsertEventGuest(EventGuest eventGuest)
        {
            if (!Exists(eventGuest).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    await context.EventGuests.AddAsync(eventGuest);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteEventGuest(EventGuest eventGuest)
        {
            if (Exists(eventGuest).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var eg = context.EventGuests
                        .FindAsync(eventGuest.EventPin, eventGuest.GuestId).Result;

                    context.EventGuests.Remove(eg);

                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion

    }
}

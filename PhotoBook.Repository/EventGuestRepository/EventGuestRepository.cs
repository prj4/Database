using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.EventGuestRepository
{
    public class EventGuestRepository : IEventGuestRepository
    {
        private PhotoBookDbContext _context;

        public EventGuestRepository(PhotoBookDbContext context)
        {
            _context = context;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.EventGuests.AnyAsync();
            return result;
        }
        private async Task<bool> Exists(EventGuest eventGuest)
        {
            bool result = await _context.EventGuests.ContainsAsync(eventGuest);
            return result;
        }
        private async Task<bool> ExistsByEventPin(int eventPin)
        {
            if (await _context.EventGuests
                .AnyAsync(eg => eg.Event_Pin == eventPin))
                return true;
            return false;
        }
        private async Task<bool> ExistsByGuestId(int guestId)
        {
            if (await _context.EventGuests
                .AnyAsync(eg => eg.Guest_Id == guestId))
                return true;
            return false;
        }


        #endregion

        #region IGuestRepository Implementation

        public async Task<IQueryable<EventGuest>> GetEventGuests()
        {
            if (IfAny().Result)
            {
                var eventGuests = await _context.EventGuests.ToListAsync();
                return eventGuests.AsQueryable();
            }
            return null;
        }

        public async Task<IQueryable<EventGuest>> GetEventGuestsByEventPin(int eventPin)
        {
            if (ExistsByEventPin(eventPin).Result)
            {
                var eventGuests = await _context.EventGuests.
                    Where(eg => eg.Event_Pin == eventPin).
                    ToListAsync();

                return eventGuests.AsQueryable();
            }

            return null;
        }

        public async Task<IQueryable<EventGuest>> GetEventGuestsByGuestId(int guestId)
        {
            if (ExistsByGuestId(guestId).Result)
            {
                var eventGuests = await _context.EventGuests.
                    Where(eg => eg.Guest_Id == guestId).
                    ToListAsync();

                return eventGuests.AsQueryable();
            }
            return null;
        }

        public async void InsertEventGuest(EventGuest eventGuest)
        {
            if (!Exists(eventGuest).Result)
            {
                await _context.EventGuests.AddAsync(eventGuest);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteEventGuestByEventPin(int eventPin)
        {
            if (ExistsByEventPin(eventPin).Result)
            {
                var eventGuest = _context.EventGuests
                    .FindAsync(eventPin).Result;

                _context.EventGuests.Remove(eventGuest);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteEventGuestByGuestId(int guestId)
        {
            if (ExistsByGuestId(guestId).Result)
            {
                var eventGuest = _context.EventGuests
                    .FindAsync(guestId).Result;

                _context.EventGuests.Remove(eventGuest);
                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateEventGuest(EventGuest eventGuest)
        {
            if (Exists(eventGuest).Result)
            {
                var entity = await _context.EventGuests
                    .Where(eg =>((eg.Event_Pin == eventGuest.Event_Pin) && 
                                 (eg.Guest_Id == eventGuest.Guest_Id)))
                    .SingleAsync();

                entity.Guest_Id = eventGuest.Guest_Id;
                entity.Event_Pin = eventGuest.Event_Pin;

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
        }
        #endregion

    }
}

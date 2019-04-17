using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;
[assembly: InternalsVisibleTo("PhotoBook.Test")]
namespace PhotoBook.Repository.EventRepository
{
    public class EventRepository : IEventRepository
    {

        private PhotoBookDbContext _context;
        public EventRepository(PhotoBookDbContext context)
        {
            _context = context;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Events.AnyAsync();
            return result;
        }


        private async Task<bool> ExistsByEvent(Event eve)
        {
            bool result = await _context.Events.AnyAsync(e => (e.HostId == eve.HostId) && (e.Pin == eve.Pin));
            return result;
        }

        private async Task<bool> ExistsByPin(string pin)
        {
            if (await _context.Events
                .AnyAsync(e => e.Pin == pin))
                return true;
            return false;
        }

        private async Task<bool> ExistsByHostId(int HostId)
        {
            if (await _context.Events
                .AnyAsync(e => e.HostId == HostId))
                return true;
            return false;
        }

        #endregion

        #region IEventRepository Implementation

        public async Task<IEnumerable<Event>> GetEvents()
        {
            if (IfAny().Result)
            {
                var events = await _context.Events.ToListAsync();
                return events.AsEnumerable();
            }
            return null;
        }

        public async Task<IEnumerable<Event>> GetEventsByHostId(int hostId)
        {
            if (ExistsByHostId(hostId).Result)
            {
                var events = await _context
                    .Events.Where(e => e.HostId == hostId)
                    .ToListAsync();
                return events.AsEnumerable();
            }
            return null;
        }

        public async Task<Event> GetEventByPin(string eventPin)
        {
            if (ExistsByPin(eventPin).Result)
            {
                var eve = await _context.Events
                    .Include(e => e.Pictures)
                    .SingleOrDefaultAsync(e => e.Pin == eventPin);
                return eve;
            }
            return null;
        }

        public async Task InsertEvent(Event eve)
        {
            if (!ExistsByEvent(eve).Result)
            {
                await _context.Events.AddAsync(eve);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEventByPin(string pin)
        {
            if (ExistsByPin(pin).Result)
            {
                var eve = _context.Events
                    .FindAsync(pin).Result;

                _context.Events.Remove(eve);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateEvent(Event eve)
        {
            if (ExistsByEvent(eve).Result)
            {
                var entity = await _context.Events.FindAsync(eve.Pin);

                entity.Description = eve.Description;
                entity.Name = eve.Name;
                entity.Pin = eve.Pin;
                entity.EndDate = eve.EndDate;
                entity.StartDate = eve.StartDate;
                entity.Location = eve.Location;

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
        }
        #endregion

    }
}

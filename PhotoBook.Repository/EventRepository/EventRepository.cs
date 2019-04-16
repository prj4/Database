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
        
        private DbContextOptions<PhotoBookDbContext> _options;

        internal EventRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _options = options;
        }


        public EventRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        #region Private Methods

        private async Task<bool> IfAny(PhotoBookDbContext context)
        {
            bool result = await context.Events.AnyAsync();
            return result;
        }


        private async Task<bool> ExistsByEvent(Event eve, PhotoBookDbContext context)
        {
            bool result = await context.Events.AnyAsync(e => (e.HostId == eve.HostId) && (e.Pin == eve.Pin));
            return result;
        }

        private async Task<bool> ExistsByPin(string pin, PhotoBookDbContext context)
        {
            if (await context.Events
                .AnyAsync(e => e.Pin == pin))
                return true;
            return false;
        }

        private async Task<bool> ExistsByHostId(int HostId, PhotoBookDbContext context)
        {
            if (await context.Events
                .AnyAsync(e => e.HostId == HostId))
                return true;
            return false;
        }

        #endregion

        #region IEventRepository Implementation

        public async Task<IEnumerable<Event>> GetEvents()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (IfAny(context).Result)
                {
                    var events = await context.Events.ToListAsync();
                    return events.AsEnumerable();
                }
            }
            return null;
        }

        public async Task<IEnumerable<Event>> GetEventsByHostId(int hostId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByHostId(hostId, context).Result)
                {
                    var events = await context.Events.Where(e => e.HostId == hostId).ToListAsync();
                    return events.AsEnumerable();
                }
            }
            return null;
        }

        public async Task<Event> GetEventByPin(string eventPin)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByPin(eventPin, context).Result)
                {
                    var eve = await context.Events
                        .FindAsync(eventPin);
                    return eve;
                }
            }
            return null;
        }

        public async Task InsertEvent(Event eve)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (!ExistsByEvent(eve, context).Result)
                {
                    await context.Events.AddAsync(eve);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteEventByPin(string pin)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByPin(pin, context).Result)
                {
                    var eve = context.Events
                        .FindAsync(pin).Result;

                    context.Events.Remove(eve);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateEvent(Event eve)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByEvent(eve, context).Result)
                {
                    var entity = await context.Events.FindAsync(eve.Pin);

                    entity.Description = eve.Description;
                    entity.Name = eve.Name;
                    entity.Pin = eve.Pin;
                    entity.EndDate = eve.EndDate;
                    entity.StartDate = eve.StartDate;
                    entity.Location = eve.Location;

                    context.Update(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion

    }
}

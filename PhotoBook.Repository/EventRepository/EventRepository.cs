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
        private DbContextOptions<PhotoBookDbContext> _options;

        internal EventRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _context = new PhotoBookDbContext(options);
        }


        public EventRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new PhotoBookDbContext(_options);
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Events.AnyAsync();
            return result;
        }
        private async Task<bool> Exists(Event eve)
        {
            bool result = await _context.Events.AnyAsync(e => e.HostId == eve.HostId); 

            return result;
        }
        private async Task<bool> Exists(int pin)
        {
            if (await _context.Events
                .AnyAsync(e=> e.Pin == pin))
                return true;
            return false;
        }
        private async Task<bool> Exists(string name)
        {
            if (await _context.Events
                .AnyAsync(e => e.Name == name))
                return true;
            return false;
        }
        

        #endregion

        #region IEventRepository Implementation

        public async Task<IQueryable<Event>> GetEvents()
        {
            if (IfAny().Result)
            {
                var events = await _context.Events.ToListAsync();
                return events.AsQueryable();
            }

            return null;
        }

        public async Task<Event> GetEvent(int eventPin)
        {
            if (Exists(eventPin).Result)
            {
                var eve = await _context.Events
                    .FindAsync(eventPin);
                return eve;
            }

            return null;
        }

        public async Task<Event> GetEvent(string name)
        {
            if (Exists(name).Result)
            {
                var eve = await _context.Events
                    .Where(x => x.Name == name)
                    .FirstOrDefaultAsync();
                return eve;
            }
            return null;
        }

        public async void InsertEvent(Event eve)
        {
            if (!Exists(eve).Result)
            {
                await _context.Events.AddAsync(eve);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteEvent(int pin)
        {
            if (Exists(pin).Result)
            {
                var eve = _context.Events
                    .FindAsync(pin).Result;

                _context.Events.Remove(eve);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteEvent(string name)
        {
            if (Exists(name).Result)
            {
                var eve = _context.Events
                    .Where(x => x.Name == name)
                    .FirstOrDefaultAsync().Result;

                _context.Events.Remove(eve);
                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateEvent(Event eve)
        {
            if (Exists(eve).Result)
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

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

        private async Task<bool> IfAny()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Events.AnyAsync();
                return result;
            }
        }
        private async Task<bool> Exists(Event eve)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Events.AnyAsync(e => e.HostId == eve.HostId);

                return result;
            }
        }
        private async Task<bool> Exists(int pin)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Events
                    .AnyAsync(e => e.Pin == pin))
                    return true;
                return false;
            }
        }
        private async Task<bool> Exists(string name)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Events
                    .AnyAsync(e => e.Name == name))
                    return true;
                return false;
            }
        }
        

        #endregion

        #region IEventRepository Implementation

        public async Task<IQueryable<Event>> GetEvents()
        {
            if (IfAny().Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var events = await context.Events.ToListAsync();
                    return events.AsQueryable();
                }
            }
            return null;
        }

        public async Task<Event> GetEvent(int eventPin)
        {
            if (Exists(eventPin).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var eve = await context.Events
                        .FindAsync(eventPin);
                    return eve;
                }
            }
            return null;
        }

        public async Task<Event> GetEvent(string name)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(name).Result)
                {
                    var eve = await context.Events
                        .Where(x => x.Name == name)
                        .FirstOrDefaultAsync();
                    return eve;
                }

                return null;
            }
        }

        public async void InsertEvent(Event eve)
        {
            if (!Exists(eve).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    await context.Events.AddAsync(eve);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteEvent(int pin)
        {
            if (Exists(pin).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var eve = context.Events
                        .FindAsync(pin).Result;

                    context.Events.Remove(eve);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteEvent(string name)
        {
            if (Exists(name).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var eve = context.Events
                        .Where(x => x.Name == name)
                        .FirstOrDefaultAsync().Result;

                    context.Events.Remove(eve);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void UpdateEvent(Event eve)
        {
            if (Exists(eve).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
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

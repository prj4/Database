using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using PhotoBook.Repository.HostRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

[assembly: InternalsVisibleTo("PhotoBook.Test")]

    namespace PhotoBook.Repository.HostRepository
{
    public class HostRepository : IHostRepository 
    {
        private DbContextOptions<PhotoBookDbContext> _options;

        /*Constructor needed for test with InMemory*/
        internal HostRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _options = options;
        }


        public HostRepository(string connectionString)
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
                bool result = await context.Hosts.AnyAsync();
                return result;
            }
        }
        private async Task<bool> Exists(Host host)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Hosts.AnyAsync(h => h.PictureTakerId == host.PictureTakerId);
                return result;
            }
        }
        private async Task<bool> Exists(int id)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Hosts
                    .AnyAsync(h => h.PictureTakerId == id))
                    return true;
                return false;
            }
        }
        private async Task<bool> Exists(string name)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Hosts
                    .AnyAsync(h => h.Name == name))
                    return true;
                return false;
            }
        }


        #endregion

        #region IHostRepository Implementation
        /// <summary>
        /// Hello
        /// </summary>
        /// <returns>Returns Iqueryable of host's, NULL if none</returns>
        public async Task<IQueryable<Host>> GetHosts()
        {
            if (IfAny().Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {
                    var hosts = await context.Hosts.ToListAsync();

                    return hosts.AsQueryable();
                }  
            }
            return null;
        }

        public async Task<Host> GetHost(int hostId)
        {
            if (Exists(hostId).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var host = await context.Hosts
                        .FindAsync(hostId);

                    return host;
                }
            }

            return null;
        }

        public async Task<Host> GetHost(string hostName)
        {
            if (Exists(hostName).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var host = await context.Hosts
                        .Where(x => x.Name == hostName)
                        .FirstOrDefaultAsync();

                    return host;
                }
            }

            return null;
        }

        public async void InsertHost(Host host)
        {
            if (!Exists(host).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    await context.Hosts.AddAsync(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteHost(int hostId)
        {
            if (Exists(hostId).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var host = context.Hosts
                        .FindAsync(hostId).Result;

                    context.Hosts.Remove(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeleteHost(string hostName)
        {
            if (Exists(hostName).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var host = context.Hosts
                        .Where(x => x.Name == hostName)
                        .FirstOrDefaultAsync().Result;

                    context.Hosts.Remove(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void UpdateHost(Host host)
        {
            if (Exists(host).Result)
            {
                using (var context = new PhotoBookDbContext(_options))
                {

                    var entity = await context.Hosts.FirstOrDefaultAsync(
                        h => h.PictureTakerId == host.PictureTakerId);

                    entity.Email = host.Email;
                    entity.Name = host.Name;

                    context.Update(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion
    }
}

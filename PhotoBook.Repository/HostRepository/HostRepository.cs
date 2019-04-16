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

        private async Task<bool> IfAny(PhotoBookDbContext context)
        {
            bool result = await context.Hosts.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByHost(Host host, PhotoBookDbContext context)
        {
            bool result = await context.Hosts.AnyAsync(h => h.PictureTakerId == host.PictureTakerId);
            return result;
        }

        private async Task<bool> ExistsById(int id, PhotoBookDbContext context)
        {
            if (await context.Hosts
                .AnyAsync(h => h.PictureTakerId == id))
                return true;
            return false;
        }

        private async Task<bool> ExistsByEmail(string email, PhotoBookDbContext context)
        {
            if (await context.Hosts
                .AnyAsync(h => h.Email == email))
                return true;
            return false;
        }



        #endregion

        #region IHostRepository Implementation
        public async Task<IEnumerable<Host>> GetHosts()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (IfAny(context).Result)
                {
                    var hosts = await context.Hosts.ToListAsync();

                    return hosts.AsQueryable();
                }
            }
            return null;
        }

        public async Task<Host> GetHostById(int hostId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(hostId, context).Result)
                {
                    var host = await context.Hosts
                        .FindAsync(hostId);

                    return host;
                }
            }
            return null;
        }

        public async Task<Host> GetHostByEmail(string email)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByEmail(email, context).Result)
                {
                    var host = await context.Hosts
                        .Where(h => h.Email == email)
                        .FirstOrDefaultAsync();

                    return host;
                }
            }
            return null;
        }



        public async Task InsertHost(Host host)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (!ExistsByHost(host,context).Result)
                {
                    await context.Hosts.AddAsync(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteHostById(int hostId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(hostId,context).Result)
                {
                    var host = context.Hosts
                        .FindAsync(hostId).Result;

                    context.Hosts.Remove(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteHostByEmail(string email)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByEmail(email,context).Result)
                {
                    var host = context.Hosts
                        .Where(x => x.Email == email)
                        .FirstOrDefaultAsync().Result;

                    context.Hosts.Remove(host);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateHost(Host host)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsByHost(host,context).Result)
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

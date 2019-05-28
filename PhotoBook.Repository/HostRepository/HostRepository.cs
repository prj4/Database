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



    namespace PhotoBook.Repository.HostRepository
{
    public class HostRepository : IHostRepository
    {
        private PhotoBookDbContext _context;
        public HostRepository(PhotoBookDbContext context)
        {
            _context = context;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Hosts.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByHost(Host host)
        {
            bool result = await _context.Hosts.AnyAsync(h => ((h.HostId == host.HostId)||(h.Email == host.Email)));
            return result;
        }

        private async Task<bool> ExistsById(int id)
        {
            if (await _context.Hosts
                .AnyAsync(h => h.HostId == id))
                return true;
            return false;
        }

        private async Task<bool> ExistsByEmail(string email)
        {
            if (await _context.Hosts
                .AnyAsync(h => h.Email == email))
                return true;
            return false;
        }



        #endregion

        #region IHostRepository Implementation

        public async Task<IEnumerable<Host>> GetHosts()
        {
            if (IfAny().Result)
            {
                var hosts = await _context.Hosts.ToListAsync();

                return hosts.AsEnumerable();
            }
            return null;
        }

        public async Task<Host> GetHostById(int hostId)
        {
            if (ExistsById(hostId).Result)
            {
                var host = await _context.Hosts
                    .FindAsync(hostId);

                return host;
            }

            return null;
        }

        public async Task<Host> GetHostByEmail(string email)
        {
            if (ExistsByEmail(email).Result)
            {
                var host = await _context.Hosts
                    .Where(h => h.Email == email)
                    .FirstOrDefaultAsync();
                return host;
            }
            return null;
        }



        public async Task InsertHost(Host host)
        {
            if (!ExistsByHost(host).Result)
                {
                    await _context.Hosts.AddAsync(host);
                    await _context.SaveChangesAsync();
                }
            
        }

        public async Task DeleteHostById(int hostId)
        {
            if (ExistsById(hostId).Result)
            {
                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    var host = _context.Hosts
                        .Include(h => h.Events).ThenInclude(e => e.Guests)
                        .Include(h => h.Events).ThenInclude(e => e.Pictures)
                        .Include(h => h.Pictures)
                        .FirstOrDefaultAsync(h => h.HostId == hostId).Result;

                    foreach (var eve in host.Events)
                    {
                        if (eve.Pictures.Count > 0)
                        {
                            _context.Pictures.RemoveRange(eve.Pictures);
                        }

                        if (eve.Guests.Count > 0)
                        {
                            _context.Guests.RemoveRange(eve.Guests);
                        }

                        _context.Events.Remove(eve);
                    }

                    _context.Hosts.Remove(host);

                    transaction.Result.Commit();
                    while (transaction.IsCompleted != true)
                    { }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteHostByEmail(string email)
        {
            
                if (ExistsByEmail(email).Result)
                {
                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    var host = _context.Hosts
                        .Include(h => h.Events).ThenInclude(e => e.Guests)
                        .Include(h => h.Events).ThenInclude(e => e.Pictures)
                        .Include(h => h.Pictures)
                        .FirstOrDefaultAsync(h => h.Email == email).Result;

                    foreach (var eve in host.Events)
                    {
                        if (eve.Pictures.Count > 0)
                        {
                            _context.Pictures.RemoveRange(eve.Pictures);
                        }

                        if (eve.Guests.Count > 0)
                        {
                            _context.Guests.RemoveRange(eve.Guests);
                        }

                        _context.Events.Remove(eve);
                    }

                    _context.Hosts.Remove(host);

                    transaction.Result.Commit();
                    while (transaction.IsCompleted != true)
                    { }
                }
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task UpdateHost(Host host)
        {
            if (ExistsByHost(host).Result)
            {
                var entity = await _context.Hosts.FirstOrDefaultAsync(
                    h => h.HostId == host.HostId);

                entity.Email = host.Email;
                entity.Name = host.Name;

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}

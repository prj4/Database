using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private async Task<bool> Exists(Host host)
        {
            bool result = await _context.Hosts.ContainsAsync(host);
            return result;
        }
        private async Task<bool> Exists(int id)
        {
            if (await _context.Hosts
                .AnyAsync(h => h.PictureTakerId == id))
                return true;
            return false;
        }
        private async Task<bool> Exists(string name)
        {
            if (await _context.Hosts
                .AnyAsync(h => h.Name == name))
                return true;
            return false;
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
                var hosts = await _context.Hosts.ToListAsync();
                
                return hosts.AsQueryable();
            }

            return null;
        }

        public async Task<Host> GetHost(int hostId)
        {
            if (Exists(hostId).Result)
            {
                var host = await _context.Hosts
                    .FindAsync(hostId);
                
                return host;
            }

            return null;
        }

        public async Task<Host> GetHost(string hostName)
        {
            if (Exists(hostName).Result)
            {
                var host = await _context.Hosts
                    .Where(x => x.Name == hostName)
                    .FirstOrDefaultAsync();
                
                return host;
            }

            return null;
        }

        public async void InsertHost(Host host)
        {
            if (!Exists(host).Result)
            {
                await _context.Hosts.AddAsync(host);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteHost(int hostId)
        {
            if (Exists(hostId).Result)
            {
                var host = _context.Hosts
                    .FindAsync(hostId).Result;

                _context.Hosts.Remove(host);
                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteHost(string hostName)
        {
            if (Exists(hostName).Result)
            {
                var host = _context.Hosts
                    .Where(x => x.Name == hostName)
                    .FirstOrDefaultAsync().Result;

                _context.Hosts.Remove(host);
                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateHost(Host host, string email)
        {
            if (Exists(host).Result)
            {
                var entity = await _context.Hosts.FirstOrDefaultAsync(
                    h => h.PictureTakerId == host.PictureTakerId);

                entity.Email = email;

                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateHost(Host host, string email, string password)
        {
            if (Exists(host).Result)
            {
                var entity = await _context.Hosts.FirstOrDefaultAsync(
                    h => h.PictureTakerId == host.PictureTakerId);

                entity.Email = email;
                entity.PW = password;

                await _context.SaveChangesAsync();
            }

            return;
        }

        
        #endregion
    }
}

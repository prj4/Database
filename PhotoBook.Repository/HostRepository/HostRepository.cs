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

        /* IHostRepository Implementation*/
        public async Task<IEnumerable<Host>> GetHosts()
        {
            if (_context.Hosts.AnyAsync().Result)
            {
                var hosts = _context.Hosts.ToListAsync().Result;
                
                return hosts;
            }

            return null;
        }

        public async Task<Host> GetHost(int hostId)
        {
            if (Exists(hostId).Result)
            {
                var host = _context.Hosts
                    .FindAsync(hostId).Result;
                
                return host;
            }

            return null;
        }

        public async Task<Host> GetHost(string hostName)
        {
            if (Exists(hostName).Result)
            {
                var host = _context.Hosts
                    .Where(x => x.Name == hostName)
                    .FirstOrDefaultAsync().Result;
                
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
                var entity = _context.Hosts.FirstOrDefault(
                    h => h.PictureTakerId == host.PictureTakerId);

                entity.Email = email;

                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateHost(Host host, string email, string password)
        {
            if (Exists(host).Result)
            {
                var entity = _context.Hosts.FirstOrDefault(
                    h => h.PictureTakerId == host.PictureTakerId);

                entity.Email = email;
                entity.PW = password;

                await _context.SaveChangesAsync();
            }

            return;
        }

        private async Task<bool> Exists(Host host)
        {
            return _context.Hosts.ContainsAsync(host).Result;
        }
        private async Task<bool> Exists(int id)
        {
            if (_context.Hosts
                .AnyAsync(h => h.PictureTakerId == id).Result)
                return true;
            return false;
        }
        private async Task<bool> Exists(string name)
        {
            if (_context.Hosts
                .AnyAsync(h => h.Name == name).Result)
                return true;
            return false;
        }
    }
}

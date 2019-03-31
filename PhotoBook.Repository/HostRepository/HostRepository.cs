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
            var hosts = _context.Hosts.ToListAsync().Result;
            await _context.SaveChangesAsync();
            return hosts;
        }

        public async Task<Host> GetHost(int hostId)
        {
            var host = _context.Hosts
                .FindAsync(hostId).Result;
            await _context.SaveChangesAsync();
            return host;
        }

        public async Task<Host> GetHost(string hostName)
        {
            var host = _context.Hosts
                .Where(x => x.Username == hostName)
                .FirstOrDefaultAsync().Result;
            await _context.SaveChangesAsync();
            return host;
        }

        public async void InsertHost(Host host)
        {
            await _context.Hosts.AddAsync(host);
            await _context.SaveChangesAsync();
        }

        public async void DeleteHost(int hostId)
        {
            var host = _context.Hosts
                .FindAsync(hostId).Result;

            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync();
        }

        public async void DeleteHost(string hostName)
        {
            var host = _context.Hosts
                .Where(x => x.Username == hostName)
                .FirstOrDefaultAsync().Result;

            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync();
        }

        public async void UpdateHost(Host host)
        {
            _context.Entry(host).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


    }
}

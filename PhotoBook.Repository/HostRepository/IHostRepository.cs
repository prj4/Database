using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.HostRepository
{
    public interface IHostRepository
    {
        Task<IQueryable<Host>> GetHosts();
        Task<Host> GetHost(int hostId);
        Task<Host> GetHost(string hostName);
        void InsertHost(Host host);
        void DeleteHost(int hostId);
        void DeleteHost(string hostName);
        void UpdateHost(Host host, string email);
        void UpdateHost(Host host, string email, string password);


    }
}

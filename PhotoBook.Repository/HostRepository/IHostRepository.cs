using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.HostRepository
{
    public interface IHostRepository
    {
        Task<IEnumerable<Host>> GetHosts();
        Task<Host> GetHost(int hostId);
        Task<Host> GetHost(string hostName);
        void InsertHost(Host host);
        void DeleteHost(int hostId);
        void DeleteHost(string hostName);
        void UpdateHost(Host host);

    }
}

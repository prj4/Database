using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

//TODO: Maybe change so dont use function overload
namespace PhotoBook.Repository.HostRepository
{
    public interface IHostRepository
    {
        Task<IEnumerable<Host>> GetHosts();
        Task<Host> GetHostById(int hostId);
        Task<Host> GetHostByEmail(string email);
        Task InsertHost(Host host);
        Task DeleteHostById(int hostId);
        Task DeleteHostByEmail(string email);
        Task UpdateHost(Host host);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.EventRepository
{
    public interface IEventRepository
    {
        Task<IQueryable<Event>> GetEvents();
        Task<Event> GetEvent(string pin);
        Task<IQueryable<Event>> GetEvents(int hostId);
        void InsertEvent(Event eve);
        void DeleteEvent(string pin);
        void UpdateEvent(Event eve);
    }
}

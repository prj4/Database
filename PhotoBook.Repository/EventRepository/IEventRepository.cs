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
        Task<IEnumerable<Event>> GetEvents();
        Task<Event> GetEventByPin(string pin);
        Task<IEnumerable<Event>> GetEventsByHostId(int hostId);
        Task InsertEvent(Event eve);
        Task DeleteEventByPin(string pin);
        Task UpdateEvent(Event eve);
    }
}

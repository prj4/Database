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
        Task<Event> GetEvent(int pin);
        Task<Event> GetEvent(string name);
        void InsertEvent(Event eve);
        void DeleteEvent(int pin);
        void DeleteEvent(string name);
        void UpdateEvent(Event eve);
    }
}

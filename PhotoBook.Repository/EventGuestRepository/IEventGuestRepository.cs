using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.EventGuestRepository
{
    public interface IEventGuestRepository
    {
        Task<IQueryable<EventGuest>> GetEventGuests();
        Task<IQueryable<EventGuest>> GetEventGuestsByEventPin(int eventPin);
        Task<IQueryable<EventGuest>> GetEventGuestsByGuestId(int guestId);
        void InsertEventGuest(EventGuest guest);
        void DeleteEventGuest(EventGuest eventGuest);
    }
}

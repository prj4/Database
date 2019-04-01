using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

namespace PhotoBook.Repository.GuestRepository
{
    public interface IGuestRepository
    {
        Task<IQueryable<Guest>> GetGuests();
        Task<Guest> GetGuest(int guestId);
        Task<Guest> GetGuest(string name);
        void InsertGuest(Guest guest);
        void DeleteGuest(int id);
        void DeleteGuest(string name);
        void UpdateGuest(Guest guest);
    }
}

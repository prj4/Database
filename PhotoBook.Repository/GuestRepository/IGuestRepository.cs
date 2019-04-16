using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

//TODO: Maybe change so dont use function overload
namespace PhotoBook.Repository.GuestRepository
{
    public interface IGuestRepository
    {
        Task<IEnumerable<Guest>> GetGuests();
        Task<Guest> GetGuestById(int guestId);
        Task<Guest> GetGuestByNameAndEventPin(string name, string eventPin);
        Task InsertGuest(Guest guest);
        Task DeleteGuestById(int id);
        Task DeleteGuestByNameAndEventPin(string name, string eventPin);
        Task UpdateGuest(Guest guest);
    }
}

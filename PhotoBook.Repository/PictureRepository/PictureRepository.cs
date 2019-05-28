using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;


namespace PhotoBook.Repository.PictureRepository
{
    public class PictureRepository : IPictureRepository
    {
        private PhotoBookDbContext _context;

        /*Constructor needed for test with InMemory*/
        


        public PictureRepository(PhotoBookDbContext context)
        {
            _context = context;
        }

        #region Private Methods

        private async Task<bool> IfAny()
        {
            bool result = await _context.Pictures.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByPicture(Picture picture)
        {
            bool result = await _context.Pictures.AnyAsync(p => p.PictureId == picture.PictureId);
            return result;
        }

        private async Task<bool> ExistsById(int id)
        {
            if (await _context.Pictures
                .AnyAsync(p => p.PictureId == id))
                return true;
            return false;
        }

        private async Task<bool> ExistsByEventPin(string eventPin)
        {
            if (await _context.Pictures
                .AnyAsync(p => p.EventPin == eventPin))
                return true;
            return false;
        }

        private async Task<bool> ExistsByEventPinAndHostId(string eventPin, int hostId)
        {
            if (await _context.Pictures
                .AnyAsync(p => (p.EventPin == eventPin) && (p.HostId == hostId)))
                return true;
            return false;
        }

        private async Task<bool> ExistsByEventPinAndGuestId(string eventPin, int guestId)
        {
            if (await _context.Pictures
                .AnyAsync(p => (p.EventPin == eventPin) && (p.GuestId == guestId)))
                return true;
            return false;
        }
        #endregion

        #region IPictureRepository Implementation

        public async Task<IEnumerable<Picture>> GetPictures()
        {
            if (await IfAny())
            {
                var pictures = await _context.Pictures.ToListAsync();

                return pictures.AsEnumerable();
            }

            return null;
        }

        public async Task<IEnumerable<Picture>> GetPicturesByEventPin(string eventPin)
        {
            if (await ExistsByEventPin(eventPin))
            {
                var pictures = await _context.Pictures.Where(
                    p => p.EventPin == eventPin).
                    ToListAsync();

                return pictures.AsEnumerable();
            }

            return null;
        }

        public async Task<IEnumerable<Picture>> GetPicturesByEventPinAndHostId(string eventPin, int hostId)
        {
            if (await ExistsByEventPinAndHostId(eventPin, hostId))
            {
                var pictures = await _context.Pictures.Where(
                        p => (p.EventPin == eventPin) && (p.HostId == hostId)).
                    ToListAsync();

                return pictures.AsEnumerable();
            }
            return null;
        }

        public async Task<IEnumerable<Picture>> GetPicturesByEventPinAndGuestId(string eventPin, int guestId)
        {
            if (await ExistsByEventPinAndGuestId(eventPin, guestId))
            {
                var pictures = await _context.Pictures.Where(
                        p => (p.EventPin == eventPin) && (p.GuestId == guestId)).
                    ToListAsync();

                return pictures.AsEnumerable();
            }
            return null;
        }

        public async Task<Picture> GetPictureById(int pictureId)
        {

            if (await ExistsById(pictureId))
            {
                var picture = await _context.Pictures
                    .FindAsync(pictureId);
                return picture;
            }
            return null;
        }


        public async Task<int> InsertPicture(Picture picture)
        {

            if (!ExistsByPicture(picture).Result)
            {
                await _context.Pictures.AddAsync(picture);
                await _context.SaveChangesAsync();

                return picture.PictureId;
            }

            return -1;
        }

        public async Task DeletePictureById(int pictureId)
        {
            if (ExistsById(pictureId).Result)
            {
                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    var picture = _context.Pictures
                        .FindAsync(pictureId).Result;

                    _context.Pictures.Remove(picture);

                    transaction.Result.Commit();
                    while (transaction.IsCompleted != true)
                    { }
                }
                await _context.SaveChangesAsync();
            }
        }

        



        #endregion
    }
}

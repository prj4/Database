using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

[assembly: InternalsVisibleTo("PhotoBook.Test")]

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

        #endregion

        #region IPictureRepository Implementation

        public async Task<IEnumerable<Picture>> GetPictures()
        {
            if (IfAny().Result)
            {
                var Pictures = await _context.Pictures.ToListAsync();

                return Pictures.AsEnumerable();
            }

            return null;
        }

        public async Task<Picture> GetPictureById(int pictureId)
        {

            if (ExistsById(pictureId).Result)
            {
                var Picture = await _context.Pictures
                    .FindAsync(pictureId);
                return Picture;
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
                var picture = _context.Pictures
                    .FindAsync(pictureId).Result;

                _context.Pictures.Remove(picture);
                await _context.SaveChangesAsync();
            }

        }




        #endregion
    }
}

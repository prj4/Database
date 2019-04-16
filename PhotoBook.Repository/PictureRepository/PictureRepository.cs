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
        private DbContextOptions<PhotoBookDbContext> _options;

        /*Constructor needed for test with InMemory*/
        internal PictureRepository(DbContextOptions<PhotoBookDbContext> options)
        {
            _options = options;
        }


        public PictureRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        #region Private Methods

        private async Task<bool> IfAny(PhotoBookDbContext context)
        {
            bool result = await context.Pictures.AnyAsync();
            return result;
        }

        private async Task<bool> ExistsByPicture(Picture picture, PhotoBookDbContext context)
        {
            bool result = await context.Pictures.AnyAsync(p => p.PictureId == picture.PictureId);
            return result;
        }

        private async Task<bool> ExistsById(int id, PhotoBookDbContext context)
        {
            if (await context.Pictures
                .AnyAsync(p => p.PictureId == id))
                return true;
            return false;
        }

        #endregion

        #region IPictureRepository Implementation

        public async Task<IEnumerable<Picture>> GetPictures()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (IfAny(context).Result)
                {
                    var Pictures = await context.Pictures.ToListAsync();

                    return Pictures.AsEnumerable();
                }
            }
            return null;
        }

        public async Task<Picture> GetPictureById(int pictureId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(pictureId, context).Result)
                {
                    var Picture = await context.Pictures
                        .FindAsync(pictureId);

                    return Picture;
                }
            }
            return null;
        }


        public async Task<int> InsertPicture(Picture picture)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (!ExistsByPicture(picture, context).Result)
                {
                    await context.Pictures.AddAsync(picture);
                    await context.SaveChangesAsync();

                    return picture.PictureId;
                }
            }
            return -1;
        }

        public async Task DeletePictureById(int pictureId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (ExistsById(pictureId, context).Result)
                {
                    var picture = context.Pictures
                        .FindAsync(pictureId).Result;

                    context.Pictures.Remove(picture);
                    await context.SaveChangesAsync();
                }
            }
        }

        

        
        #endregion
    }
}

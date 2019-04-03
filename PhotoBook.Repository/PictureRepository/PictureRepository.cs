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

        private async Task<bool> IfAny()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Pictures.AnyAsync();
                return result;
            }
        }
        private async Task<bool> Exists(Picture picture)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                bool result = await context.Pictures.AnyAsync(p => p.PictureId == picture.PictureId);
                return result;
            }
        }
        private async Task<bool> Exists(int id)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Pictures
                    .AnyAsync(p => p.PictureId == id))
                    return true;
            }

            return false;
            
        }
        private async Task<bool> Exists(string url)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (await context.Pictures
                    .AnyAsync(h => h.URL == url))
                    return true;
            }

            return false;
        }


        #endregion

        #region IPictureRepository Implementation
        
        public async Task<IQueryable<Picture>> GetPictures()
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (IfAny().Result)
                {
                    var Pictures = await context.Pictures.ToListAsync();

                    return Pictures.AsQueryable();
                }
            }

            return null;
        }

        public async Task<Picture> GetPicture(int pictureId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(pictureId).Result)
                {
                    var Picture = await context.Pictures
                        .FindAsync(pictureId);

                    return Picture;
                }
            }

            return null;
        }

        public async Task<Picture> GetPicture(string url)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(url).Result)
                {
                    var Picture = await context.Pictures
                        .Where(p => p.URL == url)
                        .FirstOrDefaultAsync();

                    return Picture;
                }
            }

            return null;
        }

        public async void InsertPicture(Picture picture)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (!Exists(picture).Result)
                {
                    await context.Pictures.AddAsync(picture);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeletePicture(int pictureId)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(pictureId).Result)
                {
                    var picture = context.Pictures
                        .FindAsync(pictureId).Result;

                    context.Pictures.Remove(picture);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void DeletePicture(string url)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(url).Result)
                {
                    var Picture = context.Pictures
                        .Where(p => p.URL == url)
                        .FirstOrDefaultAsync().Result;

                    context.Pictures.Remove(Picture);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void UpdatePicture(Picture picture)
        {
            using (var context = new PhotoBookDbContext(_options))
            {
                if (Exists(picture).Result)
                {
                    var entity = await context.Pictures.FirstOrDefaultAsync(
                        p => p.PictureId == picture.PictureId);

                    entity.URL = picture.URL;
                    entity.TakerId = picture.TakerId;

                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion
    }
}

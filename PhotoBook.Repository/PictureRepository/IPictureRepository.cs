using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBookDatabase.Model;

//TODO: Missing implementation
namespace PhotoBook.Repository.PictureRepository
{
    public interface IPictureRepository
    {
        Task<IQueryable<Picture>> GetPictures();
        Task<Picture> GetPicture(int pictureId);
        void InsertPicture(Picture picture);
        void DeletePicture(int pictureId);
        void DeletePicture(string url);
        void UpdatePicture(Picture picture);
    }
}

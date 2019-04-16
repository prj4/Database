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
        Task<IEnumerable<Picture>> GetPictures();
        Task<Picture> GetPictureById(int pictureId);
        Task<int> InsertPicture(Picture picture);
        Task DeletePictureById(int pictureId);
        
    }
}

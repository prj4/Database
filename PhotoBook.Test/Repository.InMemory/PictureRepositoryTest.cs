using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.PictureRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.InMemory
{
    [TestFixture]
    class PictureRepositoryTest
    {
        private IPictureRepository _uut;
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;

        public PictureRepositoryTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        #region Sources

        private static Picture[] PictureSource =
        {
            new Picture {EventPin = "1", TakerId = 1},
            new Picture {EventPin = "2", TakerId = 2},
            new Picture {EventPin = "3", TakerId = 3}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new PictureRepository(_InMemoryOptions);
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        #region Success Tests

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetPictures_GettingListOfPicturesAndFindingSpecific_ReturnsTrue(int pictureId)
        {
            IEnumerable<Picture> Pictures = _uut.GetPictures().Result;

            bool result = Pictures.Any(p => p.PictureId == pictureId);

            Assert.True(result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void InsertPicture_InsertPictureAndFind_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture);

            IEnumerable<Picture> Pictures = _uut.GetPictures().Result;

            bool result = Pictures.Any(g => g.PictureId == picture.PictureId);

            Assert.True(result);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public async Task InsertPicture_InsertPicture_ReturnsPictureId(int expected)
        {

            var picture = new Picture {EventPin = "1", TakerId = 1};
            var result = await _uut.InsertPicture(picture);

            Assert.AreEqual(expected,result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void GetPictureById_AddFindCompare_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture);
            var result = _uut.GetPictureById(picture.PictureId).Result;

            Assert.AreEqual(picture.PictureId, result.PictureId);
        }

        [Test, TestCaseSource("PictureSource")]
        public void DeletePictureById_InserteDeleteCheckIfNothing_EqualsNull(Picture picture)
        {
            _uut.InsertPicture(picture);

            _uut.DeletePictureById(picture.PictureId);

            IEnumerable<Picture> result = _uut.GetPictures().Result;

            Assert.AreEqual(null, result);
        }

        

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetPictureById_TryingToGetNonExistingPicture_ReturnsNull()
        {
            var result = _uut.GetPictureById(99).Result;

            Assert.AreEqual(null, result);
        }

        #endregion

    }
}

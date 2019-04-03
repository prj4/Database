using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            new Picture {PictureId = 1, EventPin = 1, TakerId = 1, URL = "wwwroot/Images/1.png"},
            new Picture {PictureId = 2, EventPin = 2, TakerId = 2, URL = "wwwroot/Images/2.png"},
            new Picture {PictureId = 3, EventPin = 3, TakerId = 3, URL = "wwwroot/Images/3.png"}
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

        [TestCase("wwwroot/Images/1.png")]
        [TestCase("wwwroot/Images/2.png")]
        [TestCase("wwwroot/Images/3.png")]
        public void GetPictures_GettingListOfPicturesAndFindingSpecific_ReturnsTrue(string url)
        {
            IQueryable<Picture> Pictures = _uut.GetPictures().Result;

            bool result = Pictures.Any(p => p.URL == url);

            Assert.True(result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void InsertPicture_InsertPictureAndFind_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture);

            IQueryable<Picture> Pictures = _uut.GetPictures().Result;

            bool result = Pictures.Any(g => g.PictureId == picture.PictureId);

            Assert.True(result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void GetPictureById_AddFindCompare_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture);
            var result = _uut.GetPicture(picture.PictureId).Result;

            Assert.AreEqual(picture.PictureId, result.PictureId);
        }

        [Test, TestCaseSource("PictureSource")]
        public void DeletePictureById_InserteDeleteCheckIfNothing_EqualsNull(Picture picture)
        {
            _uut.InsertPicture(picture);

            _uut.DeletePicture(picture.PictureId);

            IQueryable<Picture> result = _uut.GetPictures().Result;

            Assert.AreEqual(null, result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void DeletePictureByUrl_InserteDeleteCheckIfNothing_EqualsNull(Picture picture)
        {
            _uut.InsertPicture(picture);

            _uut.DeletePicture(picture.URL);

            IQueryable<Picture> result = _uut.GetPictures().Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdatePicture_InsertChangeUrlCheck_EqualsNewUrl()
        {

            var PictureBefore = new Picture {PictureId = 1, EventPin = 1, TakerId = 1, URL = "wwwroot/Images/1.png"};

            var PictureAfter = new Picture {PictureId = 1, EventPin = 1, TakerId = 1, URL = "wwwroot/Images/New1.png"};

            _uut.InsertPicture(PictureBefore);

            _uut.UpdatePicture(PictureAfter);

            var result = _uut.GetPicture(1).Result.URL;

            Assert.AreEqual("wwwroot/Images/New1.png", result);
        }

        #endregion

        #region Failure/Corner Tests

        [Test]
        public void GetPictureById_TryingToGetNonExistingPicture_ReturnsNull()
        {
            var result = _uut.GetPicture(99).Result;

            Assert.AreEqual(null, result);
        }

        #endregion

    }
}

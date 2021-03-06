﻿using System;
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

/*EN UNIT TEST AF GANGEN*/
/*Need to have a seeded Event with pin "1,2,3" to run and Host with id "1,2,3"*/
namespace PhotoBook.Test.Repository.Database
{
    [TestFixture]
    class PictureRepositoryTest
    {
        private IPictureRepository _uut;
        

        public PictureRepositoryTest()
        {
            
        }

        #region Sources

        private static Picture[] PictureSource =
        {
            new Picture {EventPin = "1", HostId = 1},
            new Picture {EventPin = "2", HostId = 2},
            new Picture {EventPin = "3", HostId = 3}
        };

        #endregion

        #region Setup and TearDown

        [SetUp]
        public void Setup()
        {
            _uut = new PictureRepository(new PhotoBookDbContext());
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
            IEnumerable<Picture> pictures = _uut.GetPictures().Result;

            bool result = pictures.Any(p => p.PictureId == pictureId);

            Assert.True(result);
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        public void GetPicturesByEventPin_GettingListOfPicturesAndFindingSpecific_ReturnsTrue(string eventPin)
        {
            IEnumerable<Picture> pictures = _uut.GetPicturesByEventPin(eventPin).Result;

            bool result = pictures.Any(p => p.EventPin == eventPin);

            Assert.True(result);
        }

        [TestCase("1",1)]
        [TestCase("2",2)]
        [TestCase("3",3)]
        public void GetPicturesByEventPinAndHostId_GettingListOfPicturesAndFindingSpecific_ReturnsTrue(string eventPin, int hostId)
        {
            IEnumerable<Picture> pictures = _uut.GetPicturesByEventPinAndHostId(eventPin,hostId).Result;

            bool result = pictures.Any(p => (p.EventPin == eventPin) && (p.HostId == hostId));

            Assert.True(result);
        }

        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("3", 3)]
        public void GetPicturesByEventPinAndGuestId_InsertingPicturesGettingListOfPicturesAndFindingSpecific_ReturnsTrue(string eventPin, int guestId)
        {
            _uut.InsertPicture(new Picture
            {
                EventPin = eventPin,
                GuestId = guestId
            }).Wait();

            IEnumerable<Picture> pictures = _uut.GetPicturesByEventPinAndGuestId(eventPin, guestId).Result;

            bool result = pictures.Any(p => (p.EventPin == eventPin) && (p.GuestId == guestId));

            Assert.True(result);
        }

        [Test, TestCaseSource("PictureSource")]
        public void InsertPicture_InsertPictureAndFind_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture).Wait();

            IEnumerable<Picture> Pictures = _uut.GetPictures().Result;

            bool result = Pictures.Any(g => g.PictureId == picture.PictureId);

            Assert.True(result);
        }


        [Test, TestCaseSource("PictureSource")]
        public void GetPictureById_AddFindCompare_ReturnsTrue(Picture picture)
        {
            _uut.InsertPicture(picture).Wait();
            var result = _uut.GetPictureById(picture.PictureId).Result;

            Assert.AreEqual(picture.PictureId, result.PictureId);
        }

        [Test, TestCaseSource("PictureSource")]
        public void DeletePictureById_InserteDeleteCheckIfNothing_EqualsNull(Picture picture)
        {
            _uut.InsertPicture(picture).Wait();

            _uut.DeletePictureById(picture.PictureId).Wait();

            var result = _uut.GetPictureById(picture.PictureId).Result;

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

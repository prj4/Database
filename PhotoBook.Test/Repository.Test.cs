﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PhotoBook.Repository.HostRepository;
using PhotoBook.Test;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
{
    public class Tests
    {
        private InMemoryDatabaseHelper _inMemoryDatabaseHelper;
        private IHostRepository _uut;

        #region Sources
        private static Host[] HostSource =
        {
            new Host {Email = "Email1@email.com", Name = "Host", Username = "Username1", PW = "PWPWPWPWPW1"},
            new Host {Email = "Email2@email.com", Name = "Host2", Username = "Username2", PW = "PWPWPWPWPW2"},
            new Host {Email = "Email3@email.com", Name = "Host3", Username = "Username3", PW = "PWPWPWPWPW3"},
        };

        #endregion

        #region Setup and TearDown




        [SetUp]
        public void Setup()
        {
            _inMemoryDatabaseHelper = new InMemoryDatabaseHelper("UnitTest");
            _uut = new HostRepository(new PhotoBookDbContext(_inMemoryDatabaseHelper._options));

            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                context.Database.EnsureDeleted();
            }
        }
        #endregion

        #region Success Tests
        [TestCase("Username1")]
        [TestCase("Username2")]
        [TestCase("Username3")]
        public void GetHosts_GettingListOfHostsAndFindingSpecific_ReturnsTrue(string username)
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabaseHelper._options))
            {
                DataSeeder dataSeeder = new DataSeeder(context);
                dataSeeder.SeedData();
            }

            IEnumerable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h.Username == username);

            Assert.True(result);
            
        }

        

        [Test, TestCaseSource("HostSource")]
        public void InsertHost__InsertHostAndFind_returnsTrue(Host host)
        {
            _uut.InsertHost(host);

            IEnumerable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h == host);

            Assert.True(result);
        }

        [Test, TestCaseSource("HostSource")]
        public void GetHostById_AddFindCompare_ReturnsTrue(Host host)
        {
            _uut.InsertHost(host);
            var tempHost = _uut.GetHost(host.PictureTakerId).Result;

            Assert.AreEqual(host,tempHost);
        }

        [Test, TestCaseSource("HostSource")]
        public void GetHostByName_AddFindCompare_ReturnsTrue(Host host)
        {
            _uut.InsertHost(host);
            var tempHost = _uut.GetHost(host.Name).Result;

            Assert.AreEqual(host, tempHost);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostById_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host);

            _uut.DeleteHost(host.PictureTakerId);

            IEnumerable<Host> result = _uut.GetHosts().Result;

            Assert.AreEqual(null,result);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostByName_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host);

            _uut.DeleteHost(host.Name);

            IEnumerable<Host> result = _uut.GetHosts().Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateHostEmailAndPW_InsertChangePWCheck_EqualsNewMailAddress()
        {
            var host = new Host
            {
                Email = "Email1@email.com",
                Name = "Host",
                Username = "Username1",
                PW = "PWPWPWPWPW1"
            };

            _uut.InsertHost(host);

            _uut.UpdateHost(host, "NewEmail1@email.com", "NewPassword9");

            var result = _uut.GetHost(host.PictureTakerId).Result;

            Assert.AreEqual("NewPassword9",result.PW);
        }

        [Test]
        public void UpdateHostEmail_InsertChangePWCheck_EqualsNewMailAddress()
        {
            var host = new Host
            {
                Email = "Email1@email.com",
                Name = "Host",
                Username = "Username1",
                PW = "PWPWPWPWPW1"
            };

            _uut.InsertHost(host);

            _uut.UpdateHost(host, "NewEmail1@email.com");

            var result = _uut.GetHost(host.PictureTakerId).Result;

            Assert.AreEqual("NewEmail1@email.com", result.Email);
        }
        #endregion

        #region Failure Tests / Corner Cases
        [Test]
        public void GetHostById_TryingToGetNonExistingHost_ReturnsNull()
        {
            var result = _uut.GetHost(1).Result;

            Assert.AreEqual(null,result);
        }

        [Test]
        public void GetHostByName_TryingToGetNonExistingHost_ReturnsNull()
        {
            var result = _uut.GetHost("Test").Result;

            Assert.AreEqual(null, result);
        }


        #endregion

    }
}
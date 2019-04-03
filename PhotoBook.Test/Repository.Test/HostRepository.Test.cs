using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PhotoBook.Repository.HostRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace Repository.Test
{
    class HostRepositoryTest
    {
        private IHostRepository _uut;
        private DbContextOptions<PhotoBookDbContext> _InMemoryOptions;

        public HostRepositoryTest()
        {
            _InMemoryOptions = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

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
            _uut = new HostRepository(_InMemoryOptions);
        }

        [TearDown]
        public void TearDown()
        {
        }
        #endregion

        #region Success Tests
        [TestCase("Username1")]
        [TestCase("Username2")]
        [TestCase("Username3")]
        public void GetHosts_GettingListOfHostsAndFindingSpecific_ReturnsTrue(string username)
        {
            IQueryable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h.Username == username);

            Assert.True(result); 
        }

        

        [Test, TestCaseSource("HostSource")]
        public void InsertHost__InsertHostAndFind_returnsTrue(Host host)
        {
            _uut.InsertHost(host);

            IQueryable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h.PictureTakerId == host.PictureTakerId);

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

            Assert.AreEqual(host.PictureTakerId, tempHost.PictureTakerId);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostById_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host);

            _uut.DeleteHost(host.PictureTakerId);

            IQueryable<Host> result = _uut.GetHosts().Result;

            Assert.AreEqual(null,result);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostByName_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host);

            _uut.DeleteHost(host.Name);

            IQueryable<Host> result = _uut.GetHosts().Result;

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
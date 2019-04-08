using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.HostRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

namespace PhotoBook.Test.Repository.InMemory
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
            new Host {Email = "Email1@email.com", Name = "Host"},
            new Host {Email = "Email2@email.com", Name = "Host2"},
            new Host {Email = "Email3@email.com", Name = "Host3"}
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
        [TestCase("Host2")]
        [TestCase("Host3")]
        public void GetHosts_GettingListOfHostsAndFindingSpecific_ReturnsTrue(string name)
        {
            IQueryable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h.Name == name);

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

            Assert.AreEqual(host.PictureTakerId,tempHost.PictureTakerId);
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
        public void UpdateHost_InsertChangeEmailCheck_EqualsNewMailAddress()
        {
            var hostBefore = new Host
            {
                PictureTakerId = 4,
                Email = "Email1@email.com",
                Name = "Host",
            };

            var hostAfter = new Host
            {
                PictureTakerId = 4,
                Email = "NewEmail1@email.com",
                Name = "Host",
            };

            _uut.InsertHost(hostBefore);

            _uut.UpdateHost(hostAfter);

            var result = _uut.GetHost(hostAfter.PictureTakerId).Result;

            Assert.AreEqual("NewEmail1@email.com", result.Email);
        }
        #endregion

        #region Failure Tests / Corner Cases
        [Test]
        public void GetHostById_TryingToGetNonExistingHost_ReturnsNull()
        {
            var result = _uut.GetHost(200).Result;

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
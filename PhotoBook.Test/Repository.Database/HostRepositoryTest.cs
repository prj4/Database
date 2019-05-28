using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PhotoBook.Repository.HostRepository;
using PhotoBookDatabase.Data;
using PhotoBookDatabase.Model;

/*EN UNIT TEST AF GANGEN, Må ikke være andre HOST i databasen*/
namespace PhotoBook.Test.Repository.Database
{
    class HostRepositoryTest
    {
        private IHostRepository _uut;
        

        public HostRepositoryTest()
        {
            
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
            _uut = new HostRepository(new PhotoBookDbContext());
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
            IEnumerable<Host> hosts = _uut.GetHosts().Result;

            bool result = hosts.Any(h => h.Name == name);

            Assert.True(result); 
        }

        [Test, TestCaseSource("HostSource")]
        public void GetHostByEmail_AddFindCompare_ReturnsTrue(Host host)
        {
            _uut.InsertHost(host).Wait();
            var result = _uut.GetHostByEmail(host.Email).Result;

            Assert.AreEqual(host.HostId, result.HostId);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostById_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host).Wait();

            _uut.DeleteHostById(host.HostId).Wait();

            var result = _uut.GetHostById(host.HostId).Result;

            Assert.AreEqual(null,result);
        }

        [Test, TestCaseSource("HostSource")]
        public void DeleteHostByEmail_InsertDeleteCheckifNothing_EqualsNull(Host host)
        {
            _uut.InsertHost(host).Wait();

            _uut.DeleteHostByEmail(host.Email).Wait();

            var result = _uut.GetHostByEmail(host.Email).Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateHost_InsertChangeEmailCheck_EqualsNewMailAddress()
        {
            var hostBefore = new Host
            {
                Email = "Email1@email.com",
                Name = "Host",
            };

            

            _uut.InsertHost(hostBefore).Wait();

            var hostAfter = new Host
            {
                HostId = hostBefore.HostId,
                Email = "NewEmail1@email.com",
                Name = "Host",
            };

            _uut.UpdateHost(hostAfter).Wait();

            var result = _uut.GetHostById(hostAfter.HostId).Result;

            Assert.AreEqual("NewEmail1@email.com", result.Email);
        }
        #endregion

        #region Failure Tests / Corner Cases
        [Test]
        public void GetHostById_TryingToGetNonExistingHost_ReturnsNull()
        {
            var result = _uut.GetHostById(200).Result;

            Assert.AreEqual(null,result);
        }

        [Test]
        public void GetHostByEmail_TryingToGetNonExistingHost_ReturnsNull()
        {
            var result = _uut.GetHostByEmail("Test").Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetHostById_CheckingOnDeleteBehaviorToEvent_ReturnsNull()
        {

            var host = new Host { Email = "Email1@email.com", Name = "Host" };

            _uut.InsertHost(host).Wait();


            var eve = new Event
            {
                Location = "Lokation4",
                Description = "Beskrivelse4",
                Name = "Event4",
                HostId = host.HostId,
                Pin = "5312",
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue
            };

            using (var context = new PhotoBookDbContext())
            {

                context.Events.Add(eve);
                _uut.DeleteHostById(host.HostId).Wait();
                var result = context.Events.FirstOrDefaultAsync(e => e.Pin == "5312").Result;

                Assert.AreEqual(null, result);
            }
        }
        #endregion


    }
}
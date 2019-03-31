using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PhotoBookDatabase.Data;

namespace Repository.Test
{
    public class Tests
    {
        private InMemoryDatabase _inMemoryDatabase;

        public class InMemoryDatabase
        {
            public DbContextOptions<PhotoBookDbContext> _options { get; }

            public InMemoryDatabase()
            {
                _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                    .UseInMemoryDatabase(databaseName: "Repository_Test")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;


            }
        }


        [SetUp]
        public void Setup()
        {
            _inMemoryDatabase = new InMemoryDatabase();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Test1()
        {
            using (var context = new PhotoBookDbContext(_inMemoryDatabase._options))
            {

            }
        }
    }
}
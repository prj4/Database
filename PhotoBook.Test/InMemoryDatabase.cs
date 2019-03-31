using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PhotoBookDatabase.Data;

namespace PhotoBook.Test
{
    public class InMemoryDatabaseHelper
    {
        public DbContextOptions<PhotoBookDbContext> _options { get; }

        public InMemoryDatabaseHelper(string databaseName)
        {
            _options = new DbContextOptionsBuilder<PhotoBookDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
    }
}

using Bank.Persistence;
using Bank.Persistence.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Application.Tests.Moq
{
    internal class BankContextTestFactory
    {
        public static IBankContext CreateSqlLiteContext() 
        {
            var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
            connection.Open();

            var builder = new DbContextOptionsBuilder<BankContext>()
                .UseSqlite(connection)
                .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));

            var context = new BankContext(builder.Options);

            return context;
        }
    }
}

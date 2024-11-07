using Dapper;
using Data.DataBase;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTests
{
    public class InMemmoryDatabaseTests
    {
        private IDbConnection _dbConnection;

        [SetUp]
        public void SetUp()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();

            DataBaseInitializer.Initialize(_dbConnection);
        }

        [Test]
        public void DatabaseTablesAreCreated()
        {
            // Check if the Credits table exists
            var checkCreditsTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Credits';";
            var creditsTable = _dbConnection.ExecuteScalar<string>(checkCreditsTableQuery);

            // Check if the Invoices table exists
            var checkInvoicesTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Invoices';";
            var invoicesTable = _dbConnection.ExecuteScalar<string>(checkInvoicesTableQuery);

            // Assert that both tables exist
            Assert.AreEqual("Credits", creditsTable);
            Assert.AreEqual("Invoices", invoicesTable);
        }

        [TearDown]
        public void TearDown()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }
    }
}

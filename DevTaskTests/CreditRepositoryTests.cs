using Data.DataBase;
using Data.Repositories;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTests
{
    [TestFixture]
    public class CreditRepositoryTests
    {
        private IDbConnection _dbConnection;
        private CreditRepository _creditRepository;

        [SetUp]
        public void SetUp()
        {
            _dbConnection = new SqliteConnection("DataSource=:memory:");
            _dbConnection.Open();

            DataBaseInitializer.Initialize(_dbConnection);

            _creditRepository = new CreditRepository(_dbConnection);
        }

        [Test]
        public async Task GetAllWithRelationsAsync_ReturnsCreditsWithInvoices()
        {
            // Act
            var credits = await _creditRepository.GetAllWithRelationsAsync(Data.Enums.CreditRelations.Invoices);

            // Assert
            Assert.IsNotNull(credits);
            Assert.AreEqual(13, credits.Count()); // Expecting 13 credits with invoices based on initialization data

            var firstCredit = credits.Where(x => x.Id == 3).First();
            Assert.AreEqual(2, firstCredit.Invoices.Count); // Expecting 2 invoices for credit with id 3
            Assert.AreEqual(1200.00, firstCredit.Invoices.First().InvoiceAmount); // Checking the first invoice amount
            Assert.AreEqual(1500.00, firstCredit.Invoices.Last().InvoiceAmount); // Checking the second invoice amount
        }

        [Test]
        public async Task GetCreditsSummaryAsync_ReturnsCorrectSummary()
        {
            // Act
            var summary = await _creditRepository.GetCreditsSummaryAsync();

            // Assert
            Assert.IsNotNull(summary);
            Assert.AreEqual(21000.00m, summary.CreditsAmountWithStatusPaid); // Total for Paid credits
            Assert.AreEqual(14500.00m, summary.CreditsAmountWithStatusAwaitingPayment); // Total for AwaitingPayment credits
            Assert.AreEqual("59.15%", summary.PercentagePaid); // Check the percentage of paid credits
            Assert.AreEqual("40.85%", summary.PercentageAwaitingPayment); // Check the percentage of awaiting payments
        }

        [TearDown]
        public void TearDown()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }
    }
}

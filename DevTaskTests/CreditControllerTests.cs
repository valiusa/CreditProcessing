using Data.Entities;
using Data;
using Data.Repositories.Interfaces;
using DevTask.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace DevTaskTests
{
    [TestFixture]
    public class CreditControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CreditController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new CreditController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_With_Credits()
        {
            // Arrange: Mock the data to be returned
            var mockedCredits = new List<Credit>
            {
                new Credit { Id = 1, CreditNumber = 1001, ClientName = "Alice", CreditAmount = 5000 },
                new Credit { Id = 2, CreditNumber = 1002, ClientName = "Bob", CreditAmount = 3000 }
            };

            _mockUnitOfWork.Setup(u => u.Credits.GetAllWithRelationsAsync(It.IsAny<Enums.CreditRelations>()))
                .ReturnsAsync(mockedCredits);

            // Act: Call the controller method
            var result = await _controller.GetAll();

            // Assert: Verify that the result is of type OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);

            // Assert: Verify that the returned value is the correct list of credits
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockedCredits, okResult.Value);

            // Assert: Verify that the repository method was called exactly once
            _mockUnitOfWork.Verify(u => u.Credits.GetAllWithRelationsAsync(It.IsAny<Enums.CreditRelations>()), Times.Once);
        }

        [Test]
        public async Task GetSummary_ReturnsOkResult_With_Summary()
        {
            // Arrange: Mock the data to be returned
            var mockedSummary = new CreditSummary
            {
                CreditsAmountWithStatusPaid = 10000,
                CreditsAmountWithStatusAwaitingPayment = 8000
            };

            _mockUnitOfWork.Setup(u => u.Credits.GetCreditsSummaryAsync())
                .ReturnsAsync(mockedSummary);

            // Act: Call the controller method
            var result = await _controller.GetSummary();

            // Assert: Verify that the result is of type OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);

            // Assert: Verify that the returned value is the correct summary
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockedSummary, okResult.Value);

            // Assert: Verify that the repository method was called exactly once
            _mockUnitOfWork.Verify(u => u.Credits.GetCreditsSummaryAsync(), Times.Once);
        }
    }
}


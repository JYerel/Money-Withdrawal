using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;

namespace NUnitTestAccount
{
    public class AccountTest
    {
        private Mock<IAccountRepository> accountServiceMock;
        private Mock<INotificationService> notificationServiceMock;

        [SetUp]
        public void SetUp()
        {
            Mock<IAccountRepository> accountServiceMock = new Mock<IAccountRepository>();
            Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
        }

        [Test]
        public void Withdraw_Valid_Amount()
        {
            // Arrange
            Account accountTest = new Account()
            {
                User = new User() { Id = Guid.NewGuid(), Name = "Jose Yerel", Email = "test@testing.com"},
                Balance = 1000m,
                Id = Guid.NewGuid(),
                PaidIn = 0m,
                Withdrawn = 0m
            };

            WithdrawMoney withdrawTest = new WithdrawMoney(accountServiceMock.Object, notificationServiceMock.Object);

            // Act
            withdrawTest.Execute(accountTest.Id, 100);

            // Assert
            Assert.AreEqual(900, accountTest.Balance);
        }

    }
}
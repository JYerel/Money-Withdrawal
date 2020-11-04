using System;
using NUnit.Framework;
using Moneybox.App;
using Moneybox.App.Features;

namespace NUnitTestAccount
{
    public class AccountTest
    {
        [Test]
        public void Withdraw_Valid_Amount()
        {
            // Arrange
            var accountTest = new Account()
            {
                Id = Guid.NewGuid(),
                User = new User(),
                Balance = 1000m
            };

            // Act
            // var result2 = new WithdrawMoney();
            // Need a constructor for the test to run but not sure if it's needed or there's another way of doing it 
            // result2.Execute(accountTest.Id,10);
            // var result = new WithdrawMoney.Execute(accountTest.Id,accountTest.Withdrawn)

            // Assert
            // Assert.AreEqual(990, accountTest.Balance);
        }

    }
}
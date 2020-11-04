using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid accountHolderId, decimal withdrawAmount)
        {
            var accountHolder = this.accountRepository.GetAccountById(accountHolderId);

            var checkNewBalance = accountHolder.Balance - withdrawAmount;
            if (checkNewBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make withdrawal");
            }

            if (checkNewBalance < 500m)
            {
                this.notificationService.NotifyFundsLow(accountHolder.User.Email);
            }

            // New validation added, extra security
            if (withdrawAmount > Account.WithdrawnLimit)
            {
                throw new InvalidOperationException("Account withdraw limit reached");
            }

            // New email notification of high transfer Amount 
            if (withdrawAmount > 3000m)
            {
                this.notificationService.NotifyTransactionAmount(accountHolder.User.Email, withdrawAmount);
            }

            accountHolder.Balance = accountHolder.Balance - withdrawAmount;
            accountHolder.Withdrawn = accountHolder.Withdrawn + withdrawAmount;

            this.accountRepository.Update(accountHolder);
        }
    }
}

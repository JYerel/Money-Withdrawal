using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid accountHolderId, Guid accountRecipientId, decimal transferAmount)
        {
            var accountHolder = this.accountRepository.GetAccountById(accountHolderId);
            var accountRecipient = this.accountRepository.GetAccountById(accountRecipientId);

            var checkNewBalance = accountHolder.Balance - transferAmount;
            if (checkNewBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            var checkNewPaidIn = accountRecipient.PaidIn + transferAmount;
            if (checkNewPaidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (checkNewBalance < 500m)
            {
                this.notificationService.NotifyFundsLow(accountHolder.User.Email);
            }

            if (Account.PayInLimit - checkNewPaidIn < 500m)
            {
                this.notificationService.NotifyApproachingPayInLimit(accountRecipient.User.Email);
            }

            // New email notification of high transfer Amount, without conflicting with PaidIn
            if (transferAmount > 3000m)
            {
              this.notificationService.NotifyTransactionAmount(accountHolder.User.Email, transferAmount);
            }


            accountHolder.Balance = accountHolder.Balance - transferAmount;
            accountHolder.Withdrawn = accountHolder.Withdrawn - transferAmount;

            accountRecipient.Balance = accountRecipient.Balance + transferAmount;
            accountRecipient.PaidIn = accountRecipient.PaidIn + transferAmount;

            this.accountRepository.Update(accountHolder);
            this.accountRepository.Update(accountRecipient);
        }
    }
}

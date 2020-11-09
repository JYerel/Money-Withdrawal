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

            accountHolder.AccountWithdraw(transferAmount, notificationService);

            accountRecipient.AccountPaidIn(transferAmount, notificationService);

            accountRepository.Update(accountHolder);
            accountRepository.Update(accountRecipient);
        }
    }
}

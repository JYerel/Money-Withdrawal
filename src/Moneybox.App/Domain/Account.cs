using System;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public const decimal WithdrawnLimit = 10000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        private void CheckAccountBalance(decimal amount)
        {
            if (Balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make withdrawal");
            }

            if (amount > Account.WithdrawnLimit)
            {
                throw new InvalidOperationException("Account withdraw limit reached");
            }
        }

        public void AccountWithdraw(decimal amount, INotificationService notificationService)
        {
            CheckAccountBalance(amount);

            NotifyClientHolder(amount, notificationService);

            AccountBalanceUpdate(amount);
        }

        public void AccountBalanceUpdate(decimal amount)
        {
            Balance = Balance - amount;
            Withdrawn = Withdrawn - amount;
        }

        public void AccountPaidIn(decimal amount, INotificationService notificationService)
        {
            CheckAccountPaidIn(amount);

            NotifyClientRecipient(amount, notificationService);

            AccountPaidInUpdate(amount);
        }


        public void AccountPaidInUpdate(decimal amount)
        {
            Balance = Balance + amount;
            PaidIn = PaidIn + amount;

        }

        public void CheckAccountPaidIn(decimal amount)
        {
            if (PaidIn + amount > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
        }

        private void NotifyClientHolder(decimal amount, INotificationService notificationService)
        {
            if (Balance - amount < 500m)
            {
                notificationService.NotifyFundsLow(User.Email);
            }

            if (amount > 3000m)
            {
                notificationService.NotifyTransactionAmount(User.Email, amount);
            }
        }

        public void NotifyClientRecipient(decimal amount, INotificationService notificationService)
        {

            decimal checkNewPaidIn = PaidIn + amount;

            if (PayInLimit - checkNewPaidIn < 500m)
            {
                notificationService.NotifyApproachingPayInLimit(User.Email);
            }
        }
    }
}

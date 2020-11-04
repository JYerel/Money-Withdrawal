namespace Moneybox.App.Domain.Services
{
    public interface INotificationService
    {
        void NotifyApproachingPayInLimit(string emailAddress);

        void NotifyFundsLow(string emailAddress);

        void NotifyTransactionAmount(string emailAddress, decimal transactionAmount);

    }
}

using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Domain.Events
{
    public class AccountCreatedEvent : IDomainEvent
    {
        public int AccountId { get; }
        public string AccountOwner { get; }
        public decimal InitialBalance { get; }

        public AccountCreatedEvent(int accountId, string accountOwner, decimal initialBalance)
        {
            AccountId = accountId;
            AccountOwner = accountOwner;
            InitialBalance = initialBalance;
        }
    }
}

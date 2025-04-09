using TransactionSystemUsingEvents.Domain.Interfaces;
using TransactionSystemUsingEvents.Domain.Models;

namespace TransactionSystemUsingEvents.Domain.Events
{
    public class DepositEvent : IDomainEvent
    {
        public int AccountId { get; }
        public string AccountOwner { get; }
        public decimal Amount { get; }

        public DepositEvent(Account account, decimal amount)
        {
            AccountId = account.Id;
            AccountOwner = account.Owner;
            Amount = amount;
        }
    }
}

using TransactionSystemUsingEvents.Domain.Interfaces;
using TransactionSystemUsingEvents.Domain.Models;

namespace TransactionSystemUsingEvents.Domain.Events
{
    public class WithdrawalEvent : IDomainEvent
    {
        public Account Account { get; }
        public string AccountOwner { get; }
        public decimal Amount { get; }

        public WithdrawalEvent(Account account, decimal amount)
        {
            Account = account;
            AccountOwner = account.Owner;
            Amount = amount;
        }
    }
}

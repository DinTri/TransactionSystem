using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Domain.Events
{
    public class TransferredEvent : IDomainEvent
    {
        public int FromAccountId { get; }
        public int ToAccountId { get; }
        public decimal Amount { get; }

        public TransferredEvent(int fromAccountId, int toAccountId, decimal amount)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
        }
    }
}

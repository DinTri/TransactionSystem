using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Domain.Models
{
    public class Account
    {
        public int Id { get; }
        public string Owner { get; }
        public decimal Balance { get; private set; }

        private List<IDomainEvent> DomainEvents { get; } = new();

        public Account(int id, string owner, decimal initialBalance)
        {
            Id = id;
            Owner = owner;
            Balance = initialBalance;

            DomainEvents.Add(new AccountCreatedEvent(Id, Owner, Balance));
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            DomainEvents.Add(new DepositEvent(this, amount));
        }

        public void Withdraw(decimal amount)
        {
            if (Balance < amount) throw new InvalidOperationException("Insufficient funds");
            Balance -= amount;
            DomainEvents.Add(new WithdrawalEvent(this, amount));
        }
    }
}

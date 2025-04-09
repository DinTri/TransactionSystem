namespace TransactionSystemCleanArchitecture.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; }

        public Account(int id, string owner, decimal balance)
        {
            Id = id;
            Owner = owner;
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
                Balance -= amount;
            else
                throw new InvalidOperationException("Insufficient balance.");
        }
        public decimal GetBalance() => Balance;
    }
}

namespace TransactionSystemTheSimplestPossible.Domain
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

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount;
            return true;
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
                Balance += amount;
        }
    }
}

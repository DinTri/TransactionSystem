namespace TransactionSystemVerticalSliceArchitecture_VSA.Domain
{
    public class Account
    {
        public int Id { get; private set; }
        public string Owner { get; private set; }
        public decimal Balance { get; private set; }

        public Account(int id, string owner, decimal initialBalance)
        {
            Id = id;
            Owner = owner;
            Balance = initialBalance;
        }

        public Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            if (Balance < amount) throw new InvalidOperationException("Insufficient balance.");
            Balance -= amount;
            return Task.CompletedTask;
        }

        public Task DepositAsync(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            Balance += amount;
            return Task.CompletedTask;
        }
    }
}

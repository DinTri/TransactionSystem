namespace TransactionSystemDDD.Domain.Entities
{
    public class Account
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public int Id { get; }
        public string? Owner { get; }
        public decimal Balance { get; private set; }

        public Account(int id, string? owner, decimal balance)
        {
            Id = id;
            Owner = owner;
            Balance = balance;
        }

        public async Task WithdrawAsync(decimal amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (Balance < amount)
                {
                    throw new InvalidOperationException("Insufficient funds.");
                }
                Balance -= amount;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                Balance += amount;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task TransferAsync(Account toAccount, decimal amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (Balance < amount)
                {
                    throw new InvalidOperationException("Insufficient funds.");
                }
                Balance -= amount;
                await toAccount.DepositAsync(amount);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public decimal GetBalance() => Balance;

        public async Task LockAsync()
        {
            await _semaphore.WaitAsync(); // Wait asynchronously for the lock
        }

        public void ReleaseLock()
        {
            _semaphore.Release(); // Release the lock
        }
    }
}

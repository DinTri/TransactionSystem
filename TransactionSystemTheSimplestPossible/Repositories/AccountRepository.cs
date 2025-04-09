using TransactionSystemTheSimplestPossible.Domain;

namespace TransactionSystemTheSimplestPossible.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<int, Account> _accounts = new();

        public async Task<Account?> GetAccountAsync(int accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return await Task.FromResult(account);
        }

        public async Task SaveAsync(Account account)
        {
            _accounts[account.Id] = account;
            await Task.CompletedTask;
        }

        public async Task<bool> DepositAsync(int accountId, decimal amount)
        {
            var account = await GetAccountAsync(accountId);
            if (account != null && amount > 0)
            {
                account.Balance += amount;
                await SaveAsync(account);
                return true;
            }
            return false;
        }

        public async Task<bool> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await GetAccountAsync(accountId);
            if (account != null && account.Balance >= amount)
            {
                account.Balance -= amount;
                await SaveAsync(account);
                return true;
            }
            return false;
        }

        public async Task<bool> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await GetAccountAsync(fromAccountId);
            var toAccount = await GetAccountAsync(toAccountId);

            if (fromAccount == null || toAccount == null || fromAccount.Balance < amount) return false;
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            await SaveAsync(fromAccount);
            await SaveAsync(toAccount);
            return true;
        }

        public async Task<Account> CreateAccountAsync(string owner, decimal initialBalance)
        {
            int newAccountId = _accounts.Count + 1;

            var newAccount = new Account(newAccountId, owner, initialBalance);

            await SaveAsync(newAccount);

            return newAccount;
        }
    }

}

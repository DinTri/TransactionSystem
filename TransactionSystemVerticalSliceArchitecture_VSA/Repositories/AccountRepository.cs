using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<int, Account> _accounts = new Dictionary<int, Account>();
        private int _nextAccountId = 1;

        public Task<Account> CreateAccountAsync(string accountName, decimal initialBalance)
        {
            var newAccount = new Account(_nextAccountId++, accountName, initialBalance);
            _accounts[newAccount.Id] = newAccount;
            return Task.FromResult(newAccount);
        }

        public Task<Account?> GetByIdAsync(int accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return Task.FromResult(account);
        }

        public async Task<Account?> GetAccountAsync(int accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return await Task.FromResult(account);
        }

        public Task SaveAsync(Account account)
        {
            _accounts[account.Id] = account;
            return Task.CompletedTask;
        }
    }
}

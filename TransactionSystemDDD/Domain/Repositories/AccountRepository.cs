using TransactionSystemDDD.Domain.Entities;
using TransactionSystemDDD.Domain.Interfaces;

namespace TransactionSystemDDD.Domain.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<int, Account?> _accounts = new();

        private int _lastAccountId = 0;
        public async Task<Account?> GetByIdAsync(int accountId)
        {
            return await Task.FromResult(_accounts.GetValueOrDefault(accountId));
        }

        public async Task<Account> CreateAccountAsync(string? accountName, decimal initialBalance)
        {
            var newAccountId = GetNextAccountId();
            var newAccount = new Account(newAccountId, accountName, initialBalance);
            await SaveAsync(newAccount);
            return newAccount;
        }

        public async Task<Account?> GetAccountAsync(int accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return await Task.FromResult(account);
        }

        public async Task SaveAsync(Account account)
        {
            await Task.Yield();
            _accounts[account.Id] = account;
        }

        private int GetNextAccountId()
        {
            _lastAccountId++;
            return _lastAccountId;
        }
    }
}

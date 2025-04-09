using TransactionSystemCleanArchitecture.Domain.Entities;
using TransactionSystemCleanArchitecture.Domain.Interfaces;

namespace TransactionSystemCleanArchitecture.Infrastructure.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<int, Account> _accounts = new();
        private int _lastAccountId = 0;
        public async Task<Account?> GetByIdAsync(int id)
        {
            _accounts.TryGetValue(id, out var account);
            return await Task.FromResult(account);
        }

        public async Task<Account> CreateAccountAsync(string? accountName, decimal initialBalance)
        {
            var newAccountId = GetNextAccountId();
            var nameToUse = accountName ?? "Lost man";
            var newAccount = new Account(newAccountId, nameToUse, initialBalance);
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
            if (!_accounts.ContainsKey(account.Id))
                _accounts.TryAdd(account.Id, account);
            else
                _accounts[account.Id] = account;

            await Task.CompletedTask;
        }
        private int GetNextAccountId()
        {
            _lastAccountId++;
            return _lastAccountId;
        }
    }
}

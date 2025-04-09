using TransactionSystemUsingEvents.Domain.Interfaces;
using TransactionSystemUsingEvents.Domain.Models;

namespace TransactionSystemUsingEvents.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<int, Account> _accounts = new Dictionary<int, Account>();
        private int _currentId = 1;

        public async Task<Account> CreateAccountAsync(string owner, decimal initialBalance)
        {
            var newAccount = new Account(_currentId++, owner, initialBalance);
            _accounts[newAccount.Id] = newAccount;
            return await Task.FromResult(newAccount);
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            _accounts.TryGetValue(id, out var account);
            return await Task.FromResult(account) ?? throw new InvalidOperationException();
        }

        public async Task SaveAsync(Account account)
        {
            _accounts[account.Id] = account;
            await Task.CompletedTask;
        }
    }
}

using TransactionSystemTheSimplestPossible.Domain;
using TransactionSystemTheSimplestPossible.Repositories;

namespace TransactionSystemTheSimplestPossible.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private int _nextId = 1;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> CreateAccountAsync(string owner, decimal initialBalance)
        {
            var accountId = _nextId++;
            var account = new Account(accountId, owner, initialBalance);
            await _accountRepository.SaveAsync(account);
            return account;
        }

        public async Task<bool> DepositAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);
            if (account == null || amount <= 0) return false;

            account.Balance += amount;
            await _accountRepository.SaveAsync(account);
            return true;
        }

        public async Task<bool> WithdrawAsync(int accountId, decimal amount)
        {
            return await _accountRepository.WithdrawAsync(accountId, amount);
        }

        public async Task<bool> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            return await _accountRepository.TransferAsync(fromAccountId, toAccountId, amount);
        }

        public async Task<decimal> CheckBalanceAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");
            }
            return account.Balance;
        }

        public async Task<Account?> GetAccountAsync(int accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId);
        }
    }
}

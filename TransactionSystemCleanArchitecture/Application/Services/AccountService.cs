using Microsoft.Extensions.Logging;
using TransactionSystemCleanArchitecture.Application.Interfaces;
using TransactionSystemCleanArchitecture.Domain.Interfaces;

namespace TransactionSystemCleanArchitecture.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository repository, ILogger<AccountService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateAccountAsync(string owner, decimal initialBalance)
        {
            var newAccount = await _repository.CreateAccountAsync(owner, initialBalance);
            await _repository.SaveAsync(newAccount);
            _logger.LogInformation("Account with ID {AccountId} and owner {Owner} created successfully.", newAccount.Id, newAccount.Owner);
            Console.WriteLine($"Account with ID {newAccount.Id} and name {newAccount.Owner} created successfully. Initial balance: {newAccount.Balance}");
        }

        public async Task DepositAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetByIdAsync(accountId);
            if (account == null)
            {
                _logger.LogError("Account with ID {AccountId} not found.", accountId);
                throw new ArgumentException("Account not found.");
            }

            account.Deposit(amount);
            await _repository.SaveAsync(account);
            _logger.LogInformation("Deposited {Amount} into account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);
            Console.WriteLine($"Deposited {amount} into account {accountId}. New balance: {account.Balance}");
        }

        public async Task WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetByIdAsync(accountId);
            if (account == null)
            {
                _logger.LogError("Account with ID {AccountId} not found.", accountId);
                throw new ArgumentException("Account not found.");
            }

            account.Withdraw(amount);
            await _repository.SaveAsync(account);
            _logger.LogInformation("Withdrawn {Amount} from account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);
            Console.WriteLine($"Withdrawn {amount} from account {accountId}. New balance: {account.Balance}");
        }

        public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await _repository.GetByIdAsync(fromAccountId);
            if (fromAccount == null)
            {
                _logger.LogError("From account with ID {FromAccountId} not found.", fromAccountId);
                throw new ArgumentException("From account not found.");
            }

            var toAccount = await _repository.GetByIdAsync(toAccountId);
            if (toAccount == null)
            {
                _logger.LogError("To account with ID {ToAccountId} not found.", toAccountId);
                throw new ArgumentException("To account not found.");
            }

            // Perform withdrawal from "from" account
            fromAccount.Withdraw(amount);

            // Perform deposit to "to" account
            toAccount.Deposit(amount);

            // Save the updated accounts
            await _repository.SaveAsync(fromAccount);
            await _repository.SaveAsync(toAccount);
            _logger.LogInformation("Transferred {Amount} from account {FromAccountId} to account {ToAccountId}. New balance: From={FromBalance}, To={ToBalance}", amount, fromAccountId, toAccountId, fromAccount.Balance, toAccount.Balance);
            Console.WriteLine($"Transferred {amount} from account {fromAccountId} to account {toAccountId}.");
        }

        public async Task<decimal> CheckBalanceAsync(int accountId)
        {
            var account = await _repository.GetAccountAsync(accountId);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            return account.GetBalance();
        }
    }
}

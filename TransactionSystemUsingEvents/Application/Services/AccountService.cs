using Microsoft.Extensions.Logging;
using TransactionSystemUsingEvents.Application.EventHandling;
using TransactionSystemUsingEvents.Application.Interfaces;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly ILogger<AccountService> _logger;
        private readonly IEventDispatcher _eventDispatcher;

        public AccountService(IAccountRepository repository, ILogger<AccountService> logger, IEventDispatcher eventDispatcher)
        {
            _repository = repository;
            _logger = logger;
            _eventDispatcher = eventDispatcher;
        }

        public async Task CreateAccountAsync(string owner, decimal initialBalance)
        {
            var newAccount = await _repository.CreateAccountAsync(owner, initialBalance);
            _logger.LogInformation("Account with ID {AccountId} and owner {Owner} created successfully.", newAccount.Id, newAccount.Owner);

            var accountCreatedEvent = new AccountCreatedEvent(newAccount.Id, newAccount.Owner, newAccount.Balance);
            await _eventDispatcher.Dispatch(accountCreatedEvent);
        }

        public async Task DepositAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetByIdAsync(accountId);
            if (account == null)
                throw new ArgumentException("Account not found.");

            account.Deposit(amount);
            await _repository.SaveAsync(account);
            _logger.LogInformation("Deposited {Amount} into account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);

            var depositEvent = new DepositEvent(account, amount);
            await _eventDispatcher.Dispatch(depositEvent);
        }

        public async Task WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetByIdAsync(accountId);
            if (account == null)
                throw new ArgumentException("Account not found.");

            account.Withdraw(amount);
            await _repository.SaveAsync(account);
            _logger.LogInformation("Withdrawn {Amount} from account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);

            var withdrawalEvent = new WithdrawalEvent(account, amount);
            await _eventDispatcher.Dispatch(withdrawalEvent);
        }

        public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await _repository.GetByIdAsync(fromAccountId);
            if (fromAccount == null)
                throw new ArgumentException("From account not found.");

            var toAccount = await _repository.GetByIdAsync(toAccountId);
            if (toAccount == null)
                throw new ArgumentException("To account not found.");

            fromAccount.Withdraw(amount);

            toAccount.Deposit(amount);

            await _repository.SaveAsync(fromAccount);
            await _repository.SaveAsync(toAccount);

            _logger.LogInformation("Transferred {Amount} from account {FromAccount} to account {ToAccount}.", amount, fromAccountId, toAccountId);

            var transferEvent = new TransferredEvent(fromAccountId, toAccountId, amount);
            await _eventDispatcher.Dispatch(transferEvent);
        }

        public async Task<decimal> CheckBalanceAsync(int accountId)
        {
            var account = await _repository.GetByIdAsync(accountId);
            if (account == null)
                throw new ArgumentException("Account not found.");

            _logger.LogInformation("Checked balance for account ID {AccountId}: {Balance}", accountId, account.Balance);
            return account.Balance;
        }

    }
}

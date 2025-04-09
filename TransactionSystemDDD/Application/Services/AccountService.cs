using Serilog;
using TransactionSystemDDD.Domain.Interfaces;

namespace TransactionSystemDDD.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero and cannot be negative.");
            }
        }

        public async Task WithdrawAsync(int accountId, decimal amount)
        {
            ValidateAmount(amount);

            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                Log.Error("Account with ID {AccountId} not found.", accountId);
                throw new ArgumentException("Account not found.");
            }

            await account.WithdrawAsync(amount);
            await _accountRepository.SaveAsync(account);

            Log.Information("Withdrawn {Amount} from account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);
        }

        public async Task DepositAsync(int accountId, decimal amount)
        {
            ValidateAmount(amount);

            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                Log.Error("Account with ID {AccountId} not found.", accountId);
                throw new ArgumentException("Account not found.");
            }

            await account.DepositAsync(amount);
            await _accountRepository.SaveAsync(account);

            Log.Information("Deposited {Amount} into account {AccountId}. New balance: {Balance}", amount, accountId, account.Balance);
        }

        public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            ValidateAmount(amount);

            var fromAccount = await _accountRepository.GetByIdAsync(fromAccountId);
            var toAccount = await _accountRepository.GetByIdAsync(toAccountId);

            if (fromAccount == null)
            {
                Log.Error("Account with ID {FromAccountId} not found.", fromAccountId);
                throw new ArgumentException("From account not found.");
            }

            if (toAccount == null)
            {
                Log.Error("Account with ID {ToAccountId} not found.", toAccountId);
                throw new ArgumentException("To account not found.");
            }

            await fromAccount.WithdrawAsync(amount);
            await toAccount.DepositAsync(amount);

            await _accountRepository.SaveAsync(fromAccount);
            await _accountRepository.SaveAsync(toAccount);

            Log.Information(
                "Transferred {Amount} from account {FromAccountId} to account {ToAccountId}. New balances: From={FromBalance}, To={ToBalance}",
                amount, fromAccountId, toAccountId, fromAccount.Balance, toAccount.Balance
            );
        }


        public async Task CreateAccountAsync(decimal initialBalance, string? accountName)
        {
            while (true)
            {
                Console.Write("Enter account ID: ");
                if (int.TryParse(Console.ReadLine(), out var accountId))
                {
                    var existingAccount = await _accountRepository.GetByIdAsync(accountId);
                    if (existingAccount == null)
                    {
                        break;
                    }

                    Console.WriteLine($"Account ID {accountId} already exists. Please choose a different ID.");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for account ID.");
                }
            }
        }

        public async Task CreateAccountAsync(string accountName, decimal initialBalance)
        {
            var newAccount = await _accountRepository.CreateAccountAsync(accountName, initialBalance);
            Console.WriteLine($"Account with ID {newAccount.Id} and name {newAccount.Owner} created successfully. Initial balance: {newAccount.Balance}");
        }

        public async Task<decimal> GetBalanceAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            return account.GetBalance();
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Transfer");
            Console.WriteLine("4. Create Account");
            Console.WriteLine("5. Check Balance");
            Console.WriteLine("6. Exit");
        }
    }

}

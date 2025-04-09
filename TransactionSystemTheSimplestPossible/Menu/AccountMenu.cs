using TransactionSystemTheSimplestPossible.Services;

namespace TransactionSystemTheSimplestPossible.Menu
{
    public class AccountMenu
    {
        private readonly IAccountService _accountService;

        public AccountMenu(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                ShowMenu();
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        DepositMoney();
                        break;
                    case "3":
                        WithdrawMoney();
                        break;
                    case "4":
                        TransferMoney();
                        break;
                    case "5":
                        CheckBalance();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Welcome to the Simple Bank Application");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Transfer Money");
            Console.WriteLine("5. Check Account Balance");
            Console.WriteLine("6. Exit");
            Console.Write("Please select an option (1-6): ");
        }

        private void CreateAccount()
        {
            Console.Clear();
            Console.WriteLine("Enter account owner's name:");
            string owner = Console.ReadLine() ?? throw new InvalidOperationException();
            Console.WriteLine("Enter initial deposit amount:");
            decimal initialBalance = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            _accountService.CreateAccountAsync(owner, initialBalance);
            Console.WriteLine($"Account for {owner} created with an initial balance of {initialBalance}.");
        }

        private void DepositMoney()
        {
            Console.Clear();
            Console.WriteLine("Enter account ID to deposit to:");
            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("Enter amount to deposit:");
            decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var result = _accountService.DepositAsync(accountId, amount).Result;
            if (result)
            {
                var updatedAccount = _accountService.GetAccountAsync(accountId).Result;
                Console.WriteLine($"Deposited {amount} to account {accountId}. New balance: {updatedAccount.Balance}");
            }
            else
            {
                Console.WriteLine("Deposit failed. Account not found or invalid amount.");
            }
        }

        private void WithdrawMoney()
        {
            Console.Clear();
            Console.WriteLine("Enter account ID to withdraw from:");
            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("Enter amount to withdraw:");
            decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var result = _accountService.WithdrawAsync(accountId, amount).Result;
            if (result)
            {
                var updatedAccount = _accountService.GetAccountAsync(accountId).Result;
                Console.WriteLine($"Withdrew {amount} from account {accountId}. New balance: {updatedAccount.Balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal failed. Insufficient balance or account not found.");
            }
        }

        private void TransferMoney()
        {
            Console.Clear();
            Console.WriteLine("Enter source account ID for transfer:");
            int fromAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("Enter destination account ID:");
            int toAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("Enter amount to transfer:");
            decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var result = _accountService.TransferAsync(fromAccountId, toAccountId, amount).Result;
            if (result)
            {
                var fromAccount = _accountService.GetAccountAsync(fromAccountId).Result;
                var toAccount = _accountService.GetAccountAsync(toAccountId).Result;
                Console.WriteLine($"Transferred {amount} from account {fromAccountId} to account {toAccountId}.");
                if (fromAccount != null)
                    Console.WriteLine($"New balance for account {fromAccountId}: {fromAccount.Balance}");
                if (toAccount != null) Console.WriteLine($"New balance for account {toAccountId}: {toAccount.Balance}");
            }
            else
            {
                Console.WriteLine("Transfer failed. Insufficient balance or invalid accounts.");
            }
        }

        private async Task CheckBalance()
        {
            Console.Write("Enter account ID: ");
            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            try
            {
                var balance = await _accountService.CheckBalanceAsync(accountId);
                Console.WriteLine($"The balance for account {accountId} is: {balance:C}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

using TransactionSystemUsingEvents.Application.Interfaces;

namespace TransactionSystemUsingEvents.Presentation
{
    public class ConsoleMenu
    {
        private readonly IAccountService _accountService;

        public ConsoleMenu(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--- Account Management ---");
                Console.ResetColor();

                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("5. Check Balance");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        await CreateAccountAsync();
                        break;
                    case "2":
                        await DepositAsync();
                        break;
                    case "3":
                        await WithdrawAsync();
                        break;
                    case "4":
                        await TransferAsync();
                        break;
                    case "5":
                        await HandleCheckBalanceAsync(_accountService);
                        break;
                    case "6":
                        return;
                    default:
                        PrintError("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        private async Task CreateAccountAsync()
        {
            Console.Write("Enter owner name: ");
            var owner = Console.ReadLine();

            var balance = ReadDecimal("Enter initial balance: ");
            if (owner != null) await _accountService.CreateAccountAsync(owner, balance);
            PrintSuccess("Account created.");
        }

        private async Task DepositAsync()
        {
            var id = ReadInt("Enter account ID: ");
            var amount = ReadDecimal("Enter deposit amount: ");
            await _accountService.DepositAsync(id, amount);
            PrintSuccess("Deposit successful.");
        }

        private async Task WithdrawAsync()
        {
            var id = ReadInt("Enter account ID: ");
            var amount = ReadDecimal("Enter withdraw amount: ");
            try
            {
                await _accountService.WithdrawAsync(id, amount);
                PrintSuccess("Withdrawal successful.");
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }
        }

        private async Task TransferAsync()
        {
            var fromId = ReadInt("Enter source account ID: ");
            var toId = ReadInt("Enter destination account ID: ");
            var amount = ReadDecimal("Enter transfer amount: ");

            try
            {
                await _accountService.TransferAsync(fromId, toId, amount);
                PrintSuccess("Transfer successful.");
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }
        }

        private static async Task HandleCheckBalanceAsync(IAccountService accountService)
        {
            Console.Write("Enter account ID to check balance: ");

            if (!int.TryParse(Console.ReadLine(), out var checkId))
            {
                Console.WriteLine("Invalid account ID.");
                return;
            }

            try
            {
                var balance = await accountService.CheckBalanceAsync(checkId);
                Console.WriteLine($"Balance for account {checkId}: {balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static int ReadInt(string prompt)
        {
            int value;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                PrintError("Invalid number. Try again: ");
                Console.Write(prompt);
            }
            return value;
        }

        private static decimal ReadDecimal(string prompt)
        {
            decimal value;
            Console.Write(prompt);
            while (!decimal.TryParse(Console.ReadLine(), out value))
            {
                PrintError("Invalid amount. Try again: ");
                Console.Write(prompt);
            }
            return value;
        }

        private static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
        }
    }
}

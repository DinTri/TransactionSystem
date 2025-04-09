using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TransactionSystemDDD.Application.Services;
using TransactionSystemDDD.Domain.Interfaces;
using TransactionSystemDDD.Domain.Repositories;

namespace TransactionSystemDDD;

static class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/account_system.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Set up dependency injection (DI)
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAccountRepository, AccountRepository>()
            .AddScoped<IAccountService, AccountService>()
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            })
            .BuildServiceProvider();

        var accountService = serviceProvider.GetRequiredService<IAccountService>();
        var repo = serviceProvider.GetRequiredService<IAccountRepository>();

        while (true)
        {
            accountService.DisplayMenu();
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter account ID: ");
                int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Enter deposit amount: ");
                decimal amount = GetValidAmountFromUser("Enter the amount to deposit: ");
                await accountService.DepositAsync(accountId, amount);
            }
            else if (choice == "2")
            {
                Console.Write("Enter account ID: ");
                int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Enter withdrawal amount: ");
                decimal amount = GetValidAmountFromUser("Enter the amount to withdraw: ");
                await accountService.WithdrawAsync(accountId, amount);
            }
            else if (choice == "3")
            {
                Console.Write("Enter from account ID: ");
                int fromAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Enter to account ID: ");
                int toAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Enter transfer amount: ");
                decimal amount = GetValidAmountFromUser("Enter the amount to transfer: ");
                await accountService.TransferAsync(fromAccountId, toAccountId, amount);
            }
            else if (choice == "4")
            {
                Console.Write("Enter account name: ");
                string? accountName = Console.ReadLine();
                Console.Write("Enter initial balance: ");
                decimal initialBalance = GetValidAmountFromUser("Enter the initial amount: ");

                Console.WriteLine("Do you want to (1) manually input account ID or (2) auto-generate account ID?");
                var idChoice = Console.ReadLine();

                if (idChoice == "1")
                {
                    await accountService.CreateAccountAsync(initialBalance, accountName);
                }
                else if (idChoice == "2")
                {
                    if (accountName != null) await accountService.CreateAccountAsync(accountName, initialBalance);
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            else if (choice == "5")
            {
                Console.Write("Enter account ID: ");
                int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                try
                {
                    var balance = await accountService.GetBalanceAsync(accountId);
                    Console.WriteLine($"The account balance is: {balance:C}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else if (choice == "6")
            {
                Console.WriteLine("Exiting the application.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select again.");
            }

            var account1Final = await repo.GetByIdAsync(1);
            var account2Final = await repo.GetByIdAsync(2);
            if (account1Final != null)
                Console.WriteLine($"Account 1 Balance: {account1Final.Balance}");
            if (account2Final != null)
                Console.WriteLine($"Account 2 Balance: {account2Final.Balance}");
        }

        await Log.CloseAndFlushAsync();
    }

    static decimal GetValidAmountFromUser(string prompt)
    {
        decimal amount = 0;
        bool validAmount = false;

        while (!validAmount)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine() ?? throw new InvalidOperationException();

            if (decimal.TryParse(input, out amount) && amount > 0)
            {
                validAmount = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid positive number (digits only).");
            }
        }

        return amount;
    }
}

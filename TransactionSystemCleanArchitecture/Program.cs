using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TransactionSystemCleanArchitecture.Application.Interfaces;
using TransactionSystemCleanArchitecture.Application.Services;
using TransactionSystemCleanArchitecture.Domain.Interfaces;
using TransactionSystemCleanArchitecture.Infrastructure.Data;

namespace TransactionSystemCleanArchitecture
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/account_system.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAccountRepository, AccountRepository>()
                .AddScoped<IAccountService, AccountService>()
                .AddLogging(builder => builder.AddSerilog())
                .BuildServiceProvider();

            var accountService = serviceProvider.GetRequiredService<IAccountService>();

            Console.WriteLine("Do you want to (1) Create Account, (2) Withdraw, (3) Deposit, (4) Transfer?");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Enter account owner name: ");
                        string owner = Console.ReadLine() ?? throw new InvalidOperationException();
                        Console.Write("Enter initial balance: ");
                        decimal balance = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                        await accountService.CreateAccountAsync(owner, balance);
                        break;
                    }
                case "2":
                    {
                        Console.Write("Enter account ID: ");
                        int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        Console.Write("Enter withdraw amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                        await accountService.WithdrawAsync(accountId, amount);
                        break;
                    }
                case "3":
                    {
                        Console.Write("Enter account ID: ");
                        int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        Console.Write("Enter deposit amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                        await accountService.DepositAsync(accountId, amount);
                        break;
                    }
                case "4":
                    {
                        Console.Write("Enter from account ID: ");
                        int fromAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        Console.Write("Enter to account ID: ");
                        int toAccountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        Console.Write("Enter transfer amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                        await accountService.TransferAsync(fromAccountId, toAccountId, amount);
                        break;
                    }
                case "5":
                    {
                        Console.Write("Enter account ID: ");
                        int accountId = int.Parse(Console.ReadLine());

                        try
                        {
                            var balance = await accountService.CheckBalanceAsync(accountId);
                            Console.WriteLine($"The account balance is: {balance:C}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    }
                default:
                    Console.WriteLine("Invalid choice. Exiting...");
                    break;
            }

            await Log.CloseAndFlushAsync();
        }
    }
}

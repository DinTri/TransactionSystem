using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CheckBalance;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CreateAccount;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Deposit;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Transfer;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Withdraw;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA
{
    internal class Program
    {
        protected Program() { }

        static async Task Main(string[] args)
        {
            // Setup Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/account_system.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Setup Dependency Injection (DI)
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAccountRepository, AccountRepository>()
                .AddLogging(builder => builder.AddSerilog())
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>())  // Use this to auto-register handlers
                .BuildServiceProvider();

            // Get the Mediator from DI container
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("5. Check Balance");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option (1-5): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        {
                            Console.Write("Enter account name: ");
                            string accountName = Console.ReadLine() ?? throw new InvalidOperationException();
                            Console.Write("Enter initial balance: ");
                            decimal initialBalance = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                            var createAccountCommand = new CreateAccountCommand(accountName, initialBalance);
                            var account = await mediator.Send(createAccountCommand);

                            Console.WriteLine($"Account created with ID: {account.Id}");
                            break;
                        }
                    case "2":
                        {
                            Console.Write("Enter account ID: ");
                            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                            Console.Write("Enter withdraw amount: ");
                            decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                            var withdrawCommand = new WithdrawCommand(accountId, amount);
                            await mediator.Send(withdrawCommand);

                            Console.WriteLine("Withdraw successful.");
                            break;
                        }
                    case "3":
                        {
                            Console.Write("Enter account ID: ");
                            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                            Console.Write("Enter deposit amount: ");
                            decimal amount = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                            var depositCommand = new DepositCommand(accountId, amount);
                            await mediator.Send(depositCommand);

                            Console.WriteLine("Deposit successful.");
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

                            var transferCommand = new TransferCommand(fromAccountId, toAccountId, amount);
                            await mediator.Send(transferCommand);

                            Console.WriteLine("Transfer successful.");
                            break;
                        }
                    case "5":
                        {
                            Console.Write("Enter account ID: ");
                            int accountId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                            var checkBalanceCommand = new CheckBalanceCommand(accountId);
                            var balance = await mediator.Send(checkBalanceCommand);

                            Console.WriteLine($"Account balance: {balance}");
                            break;
                        }
                    case "6":
                        {
                            Console.WriteLine("Exiting the application...");
                            exit = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid option, please choose a valid menu option.");
                            break;
                        }
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }

            // Close Serilog
            await Log.CloseAndFlushAsync();
        }
    }

}

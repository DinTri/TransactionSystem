using Microsoft.Extensions.DependencyInjection;
using TransactionSystemTheSimplestPossible.Menu;
using TransactionSystemTheSimplestPossible.Repositories;
using TransactionSystemTheSimplestPossible.Services;

namespace TransactionSystemTheSimplestPossible
{
    static class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAccountRepository, AccountRepository>()  // Register IAccountRepository with its implementation
                .AddSingleton<IAccountService, AccountService>()        // Register IAccountService with its implementation
                .AddSingleton<AccountMenu>()                             // Register AccountMenu
                .BuildServiceProvider();                                  // Build the service provider

            // Get the AccountMenu instance from DI container
            var menu = serviceProvider.GetService<AccountMenu>();

            // Run the menu to start the application
            menu.Run();
        }
    }
}
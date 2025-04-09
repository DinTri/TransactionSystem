using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TransactionSystemUsingEvents.Application.EventHandlers;
using TransactionSystemUsingEvents.Application.EventHandling;
using TransactionSystemUsingEvents.Application.Interfaces;
using TransactionSystemUsingEvents.Application.Services;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;
using TransactionSystemUsingEvents.Infrastructure.Repositories;
using TransactionSystemUsingEvents.Presentation;

namespace TransactionSystemUsingEvents;

static class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/account_log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Set up DI
        var services = new ServiceCollection();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        services.AddSingleton<IAccountRepository, AccountRepository>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.AddSingleton<IAccountService, AccountService>();
        services.AddSingleton<ConsoleMenu>();

        // Register domain event handlers
        services.AddTransient<IDomainEventHandler<AccountCreatedEvent>, AccountCreatedEventHandler>();
        services.AddTransient<IDomainEventHandler<DepositEvent>, MoneyDepositedEventHandler>();
        services.AddTransient<IDomainEventHandler<WithdrawalEvent>, MoneyWithdrawnEventHandler>();
        services.AddTransient<IDomainEventHandler<TransferredEvent>, MoneyTransferredEventHandler>();

        // Build provider
        var serviceProvider = services.BuildServiceProvider();

        var menu = serviceProvider.GetRequiredService<ConsoleMenu>();
        await menu.RunAsync();
    }
}
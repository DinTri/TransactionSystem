using Microsoft.Extensions.Logging;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Application.EventHandlers
{
    public class MoneyDepositedEventHandler : IDomainEventHandler<DepositEvent>
    {
        private readonly ILogger<MoneyDepositedEventHandler> _logger;

        public MoneyDepositedEventHandler(ILogger<MoneyDepositedEventHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleAsync(DepositEvent @event)
        {
            _logger.LogInformation("Deposited {Amount} to account {AccountId} (Owner: {Owner})",
                @event.Amount, @event.AccountId, @event.AccountOwner);
            Console.WriteLine($"[EVENT] Deposited {@event.Amount} into Account ID {@event.AccountId}. New Balance: {@event.AccountOwner}");
            return Task.CompletedTask;
        }
    }

}

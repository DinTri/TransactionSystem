using Microsoft.Extensions.Logging;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Application.EventHandlers
{
    public class AccountCreatedEventHandler : IDomainEventHandler<AccountCreatedEvent>
    {
        private readonly ILogger<AccountCreatedEventHandler> _logger;

        public AccountCreatedEventHandler(ILogger<AccountCreatedEventHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleAsync(AccountCreatedEvent @event)
        {
            _logger.LogInformation("Handled AccountCreatedEvent: Account {AccountId}, Owner: {Owner}, Initial Balance: {Balance}",
                @event.AccountId, @event.AccountOwner, @event.InitialBalance);
            Console.WriteLine($"[EVENT] Account created: ID={@event.AccountId}, Owner={@event.AccountOwner}, Balance={@event.InitialBalance}");
            return Task.CompletedTask;
        }
    }

}

using Microsoft.Extensions.Logging;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Application.EventHandlers
{
    public class MoneyWithdrawnEventHandler : IDomainEventHandler<WithdrawalEvent>
    {
        private readonly ILogger<MoneyWithdrawnEventHandler> _logger;

        public MoneyWithdrawnEventHandler(ILogger<MoneyWithdrawnEventHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleAsync(WithdrawalEvent @event)
        {
            _logger.LogInformation("Withdrawn {Amount} from account {AccountId} (Owner: {Owner})",
                @event.Amount, @event.Account, @event.AccountOwner);
            Console.WriteLine($"[EVENT] Withdrawn {@event.Amount} from Account ID {@event.Account} accountOwner {@event.AccountOwner}. Balance: {@event.Amount}");
            return Task.CompletedTask;
        }
    }

}

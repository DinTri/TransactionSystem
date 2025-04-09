using Microsoft.Extensions.Logging;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;

namespace TransactionSystemUsingEvents.Application.EventHandlers
{
    public class MoneyTransferredEventHandler : IDomainEventHandler<TransferredEvent>
    {
        private readonly ILogger<MoneyTransferredEventHandler> _logger;

        public MoneyTransferredEventHandler(ILogger<MoneyTransferredEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(TransferredEvent @event)
        {
            _logger.LogInformation("Transferred {Amount} from account {FromAccountId} to {ToAccountId}",
                @event.Amount, @event.FromAccountId, @event.ToAccountId);
            Console.WriteLine($"[EVENT] Transferred {@event.Amount} from Account {@event.FromAccountId} to {@event.ToAccountId}");
            return Task.CompletedTask;
        }
    }

}

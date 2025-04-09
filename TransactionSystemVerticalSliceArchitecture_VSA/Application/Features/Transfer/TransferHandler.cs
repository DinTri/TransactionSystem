using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Transfer
{
    public class TransferHandler : IRequestHandler<TransferCommand, Unit>
    {
        private readonly IAccountRepository _repository;

        public TransferHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(TransferCommand command, CancellationToken cancellationToken)
        {
            var fromAccount = await _repository.GetByIdAsync(command.FromAccountId);
            var toAccount = await _repository.GetByIdAsync(command.ToAccountId);
            if (fromAccount != null && fromAccount.Balance < command.Amount)
            {
                throw new ArgumentException("Insufficient balance.");
            }
            if (fromAccount == null) throw new ArgumentException("From account not found.");
            if (toAccount == null) throw new ArgumentException("To account not found.");

            await fromAccount.WithdrawAsync(command.Amount);
            await toAccount.DepositAsync(command.Amount);

            await _repository.SaveAsync(fromAccount);
            await _repository.SaveAsync(toAccount);

            return Unit.Value;
        }
    }
}

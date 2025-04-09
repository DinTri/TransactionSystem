using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Deposit
{
    public class DepositHandler : IRequestHandler<DepositCommand, Unit>
    {
        private readonly IAccountRepository _repository;

        public DepositHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByIdAsync(request.AccountId);
            if (account == null) throw new ArgumentException("Account not found.");

            await account.DepositAsync(request.Amount);
            await _repository.SaveAsync(account);

            return Unit.Value;
        }
    }
}

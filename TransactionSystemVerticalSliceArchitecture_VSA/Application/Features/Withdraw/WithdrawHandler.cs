using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Withdraw
{
    public class WithdrawHandler : IRequestHandler<WithdrawCommand, Unit>
    {
        private readonly IAccountRepository _repository;

        public WithdrawHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByIdAsync(request.AccountId);
            if (account == null) throw new ArgumentException("Account not found.");

            await account.WithdrawAsync(request.Amount);
            await _repository.SaveAsync(account);

            return Unit.Value;
        }
    }
}

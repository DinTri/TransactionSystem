using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CheckBalance
{
    public class CheckBalanceCommandHandler : IRequestHandler<CheckBalanceCommand, decimal>
    {
        private readonly IAccountRepository _accountRepository;

        public CheckBalanceCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<decimal> Handle(CheckBalanceCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountAsync(request.AccountId);
            if (account == null)
                throw new InvalidOperationException("Account not found");

            return account.Balance;
        }
    }

}

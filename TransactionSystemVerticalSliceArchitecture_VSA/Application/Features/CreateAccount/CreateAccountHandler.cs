using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly IAccountRepository _repository;

        public CreateAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Validate the balance
            if (request.InitialBalance < 0)
            {
                throw new ArgumentException("Invalid balance");
            }

            // Create the account if balance is valid
            var account = new Account(1, request.AccountName, request.InitialBalance);

            // Persist the account
            await _repository.CreateAccountAsync(account.Owner, account.Balance);

            return account;
        }
    }
}

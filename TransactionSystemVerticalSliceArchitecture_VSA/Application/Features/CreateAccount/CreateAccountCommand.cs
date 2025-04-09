using MediatR;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CreateAccount
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public string AccountName { get; set; }
        public decimal InitialBalance { get; set; }

        public CreateAccountCommand(string accountName, decimal initialBalance)
        {
            AccountName = accountName;
            InitialBalance = initialBalance;
        }
    }
}

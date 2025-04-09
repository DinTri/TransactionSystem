using MediatR;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CheckBalance
{
    public class CheckBalanceCommand : IRequest<decimal>
    {
        public int AccountId { get; }

        public CheckBalanceCommand(int accountId)
        {
            AccountId = accountId;
        }
    }

}

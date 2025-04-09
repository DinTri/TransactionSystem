using MediatR;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Deposit
{
    public class DepositCommand : IRequest<Unit>
    {
        public int AccountId { get; }
        public decimal Amount { get; }

        public DepositCommand(int accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}

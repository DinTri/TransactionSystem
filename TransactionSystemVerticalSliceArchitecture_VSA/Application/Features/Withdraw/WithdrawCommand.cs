using MediatR;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Withdraw
{
    public class WithdrawCommand : IRequest<Unit>
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

        public WithdrawCommand(int accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}

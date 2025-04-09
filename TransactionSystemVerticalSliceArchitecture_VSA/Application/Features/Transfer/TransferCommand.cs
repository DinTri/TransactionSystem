using MediatR;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Transfer
{
    public class TransferCommand : IRequest<Unit>
    {
        public int FromAccountId { get; }
        public int ToAccountId { get; }
        public decimal Amount { get; }

        public TransferCommand(int fromAccountId, int toAccountId, decimal amount)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
        }
    }
}

namespace TransactionSystemCleanArchitecture.Application.Interfaces
{
    public interface IAccountService
    {
        Task CreateAccountAsync(string owner, decimal initialBalance);
        Task DepositAsync(int accountId, decimal amount);
        Task WithdrawAsync(int accountId, decimal amount);
        Task TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task<decimal> CheckBalanceAsync(int accountId);
    }
}

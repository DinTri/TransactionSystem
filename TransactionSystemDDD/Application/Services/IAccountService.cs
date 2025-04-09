namespace TransactionSystemDDD.Application.Services
{
    public interface IAccountService
    {
        Task WithdrawAsync(int accountId, decimal amount);
        Task DepositAsync(int accountId, decimal amount);
        Task TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task CreateAccountAsync(decimal initialBalance, string? accountName);
        Task CreateAccountAsync(string accountName, decimal initialBalance);
        Task<decimal> GetBalanceAsync(int accountId);
        void DisplayMenu();
    }
}

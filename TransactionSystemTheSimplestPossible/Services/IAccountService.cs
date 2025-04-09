using TransactionSystemTheSimplestPossible.Domain;

namespace TransactionSystemTheSimplestPossible.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(string owner, decimal initialBalance);
        Task<bool> DepositAsync(int accountId, decimal amount);
        Task<bool> WithdrawAsync(int accountId, decimal amount);
        Task<bool> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task<decimal> CheckBalanceAsync(int accountId);
        Task<Account?> GetAccountAsync(int accountId);
    }

}

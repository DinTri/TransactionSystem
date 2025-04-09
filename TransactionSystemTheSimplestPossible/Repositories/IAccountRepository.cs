using TransactionSystemTheSimplestPossible.Domain;

namespace TransactionSystemTheSimplestPossible.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountAsync(int accountId);
        Task<Account> CreateAccountAsync(string owner, decimal initialBalance);
        Task<bool> DepositAsync(int accountId, decimal amount);
        Task<bool> WithdrawAsync(int accountId, decimal amount);
        Task<bool> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task SaveAsync(Account account);
    }

}

using TransactionSystemVerticalSliceArchitecture_VSA.Domain;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccountAsync(string accountName, decimal initialBalance);
        Task<Account?> GetByIdAsync(int accountId);
        Task<Account?> GetAccountAsync(int accountId);
        Task SaveAsync(Account account);
    }
}

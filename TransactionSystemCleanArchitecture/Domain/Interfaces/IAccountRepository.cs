using TransactionSystemCleanArchitecture.Domain.Entities;

namespace TransactionSystemCleanArchitecture.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<Account> CreateAccountAsync(string? accountName, decimal initialBalance);
        Task<Account?> GetAccountAsync(int accountId);
        Task SaveAsync(Account account);
    }
}

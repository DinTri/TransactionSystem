using TransactionSystemDDD.Domain.Entities;

namespace TransactionSystemDDD.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int accountId);
        Task<Account> CreateAccountAsync(string? accountName, decimal initialBalance);
        Task<Account?> GetAccountAsync(int accountId);
        Task SaveAsync(Account account);
    }
}

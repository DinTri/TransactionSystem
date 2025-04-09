using TransactionSystemUsingEvents.Domain.Models;

namespace TransactionSystemUsingEvents.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<Account> CreateAccountAsync(string owner, decimal initialBalance);
        Task SaveAsync(Account account);
    }
}

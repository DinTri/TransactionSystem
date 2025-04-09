using Moq;
using TransactionSystemDDD.Domain.Entities;
using TransactionSystemDDD.Domain.Interfaces;

namespace TransactionSystemDDD_Test
{
    public class AccountRepositoryTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new();

        [Fact]
        public async Task GetByIdAsync_ValidAccountId_ReturnsAccount()
        {
            // Arrange
            var accountId = 1;
            var expectedAccount = new Account(accountId, "Trifon Dinev", 6000);

            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId))
                                  .ReturnsAsync(expectedAccount);

            // Act
            var result = await _accountRepositoryMock.Object.GetByIdAsync(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount.Id, result?.Id);
            Assert.Equal(expectedAccount.Owner, result?.Owner);
            Assert.Equal(expectedAccount.Balance, result?.Balance);
        }

        [Fact]
        public async Task CreateAccountAsync_CreatesNewAccount_ReturnsNewAccount()
        {
            // Arrange
            var accountId = 2;
            var accountName = "Melissa Thompson";
            var initialBalance = 1500m;

            var newAccount = new Account(accountId, accountName, initialBalance);

            _accountRepositoryMock.Setup(repo => repo.CreateAccountAsync(accountName, initialBalance))
                                  .ReturnsAsync(newAccount);

            // Act
            var result = await _accountRepositoryMock.Object.CreateAccountAsync(accountName, initialBalance);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountId, result.Id);
            Assert.Equal(accountName, result.Owner);
            Assert.Equal(initialBalance, result.Balance);
        }

        [Fact]
        public async Task SaveAsync_Account_SavesSuccessfully()
        {
            // Arrange
            var account = new Account(3, "Hannah Smith", 1000);

            // Act
            await _accountRepositoryMock.Object.SaveAsync(account);

            // Assert
            _accountRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Account>()), Times.Once);
        }
    }
}

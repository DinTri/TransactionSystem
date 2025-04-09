using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionSystemCleanArchitecture.Application.Services;
using TransactionSystemCleanArchitecture.Domain.Entities;
using TransactionSystemCleanArchitecture.Domain.Interfaces;

namespace TransactionSystemCleanArchitecture_Test
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly Mock<ILogger<AccountService>> _mockLogger;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();
            _mockLogger = new Mock<ILogger<AccountService>>();
            _accountService = new AccountService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldCreateAccountSuccessfully()
        {
            // Arrange
            var Id = 1;
            var owner = "John Doe";
            var initialBalance = 1000m;

            var newAccount = new Account(Id, owner, initialBalance);

            _mockRepository.Setup(repo => repo.CreateAccountAsync(owner, initialBalance))
                .ReturnsAsync(newAccount);

            // Act
            await _accountService.CreateAccountAsync(owner, initialBalance);

            // Assert
            _mockRepository.Verify(repo => repo.CreateAccountAsync(owner, initialBalance), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task DepositAsync_ShouldDepositSuccessfully()
        {
            // Arrange
            var accountId = 1;
            var amount = 1500m;
            var account = new Account(accountId, "Trifon Dinev", 1000m);

            _mockRepository.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            await _accountService.DepositAsync(accountId, amount);

            // Assert
            account.Balance.Should().Be(2500m);
            _mockRepository.Verify(repo => repo.SaveAsync(account), Times.Once);
        }

        [Fact]
        public async Task DepositAsync_ShouldThrowArgumentException_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            var amount = 500m;

            _mockRepository.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.DepositAsync(accountId, amount));
        }

        [Fact]
        public async Task WithdrawAsync_ShouldWithdrawSuccessfully()
        {
            // Arrange
            var accountId = 1;
            var amount = 500m;
            var account = new Account(accountId, "Trifon Dinev", 1000m);

            _mockRepository.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            await _accountService.WithdrawAsync(accountId, amount);

            // Assert
            account.Balance.Should().Be(500m);
            _mockRepository.Verify(repo => repo.SaveAsync(account), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldThrowArgumentException_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            var amount = 500m;

            _mockRepository.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account)null); // Simulating account not found

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.WithdrawAsync(accountId, amount));
        }

        [Fact]
        public async Task TransferAsync_ShouldTransferSuccessfully()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 500m;

            var fromAccount = new Account(fromAccountId, "Trifon Dinev", 1000m);
            var toAccount = new Account(toAccountId, "Melissa Thompson", 2000m);

            _mockRepository.Setup(repo => repo.GetByIdAsync(fromAccountId))
                .ReturnsAsync(fromAccount);

            _mockRepository.Setup(repo => repo.GetByIdAsync(toAccountId))
                .ReturnsAsync(toAccount);

            // Act
            await _accountService.TransferAsync(fromAccountId, toAccountId, amount);

            // Assert
            fromAccount.Balance.Should().Be(500m);
            toAccount.Balance.Should().Be(2500m);
            _mockRepository.Verify(repo => repo.SaveAsync(fromAccount), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(toAccount), Times.Once);
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowArgumentException_WhenFromAccountNotFound()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 500m;

            _mockRepository.Setup(repo => repo.GetByIdAsync(fromAccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.TransferAsync(fromAccountId, toAccountId, amount));
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowArgumentException_WhenToAccountNotFound()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 500m;

            var fromAccount = new Account(fromAccountId, "Trifon Dinev", 1000m);

            _mockRepository.Setup(repo => repo.GetByIdAsync(fromAccountId))
                .ReturnsAsync(fromAccount);
            _mockRepository.Setup(repo => repo.GetByIdAsync(toAccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.TransferAsync(fromAccountId, toAccountId, amount));
        }
    }
}

﻿using Moq;
using TransactionSystemDDD.Application.Services;
using TransactionSystemDDD.Domain.Entities;
using TransactionSystemDDD.Domain.Interfaces;

namespace TransactionSystemDDD_Test
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountService = new AccountService(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task DepositAsync_ValidAmount_UpdatesBalance()
        {
            // Arrange
            var accountId = 1;
            var initialBalance = 1000m;
            var depositAmount = 500m;
            var expectedBalance = initialBalance + depositAmount;

            var account = new Account(accountId, "Trifon Dinev", initialBalance);
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            await _accountService.DepositAsync(accountId, depositAmount);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
            _accountRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_ValidAmount_UpdatesBalance()
        {
            // Arrange
            var accountId = 1;
            var initialBalance = 1000m;
            var withdrawAmount = 200m;
            var expectedBalance = initialBalance - withdrawAmount;

            var account = new Account(accountId, "Trifon Dinev", initialBalance);
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            await _accountService.WithdrawAsync(accountId, withdrawAmount);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
            _accountRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task TransferAsync_ValidAmount_UpdatesBothAccountsBalance()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = 300m;

            var fromAccount = new Account(fromAccountId, "Trifon Dinev", 1000m);
            var toAccount = new Account(toAccountId, "Melissa Thompson", 500m);

            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(fromAccountId))
                .ReturnsAsync(fromAccount);
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(toAccountId))
                .ReturnsAsync(toAccount);

            // Act
            await _accountService.TransferAsync(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.Equal(700m, fromAccount.Balance);
            Assert.Equal(800m, toAccount.Balance);
            _accountRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Account>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAccountAsync_WithAutoGeneratedId_ShouldCreateAccountSuccessfully()
        {
            // Arrange
            var mockRepo = new Mock<IAccountRepository>();
            var expectedAccount = new Account(123, "Trifon", 100);
            mockRepo.Setup(r => r.CreateAccountAsync("Trifon", 100)).ReturnsAsync(expectedAccount);

            var service = new AccountService(mockRepo.Object);

            // Act
            await service.CreateAccountAsync("Trifon", 100);

            // Assert
            mockRepo.Verify(r => r.CreateAccountAsync("Trifon", 100), Times.Once);
        }

        [Fact]
        public async Task CreateAccountAsync_WithPrompt_ShouldRetryOnInvalidInputAndDuplicateId()
        {
            // Arrange
            var mockRepo = new Mock<IAccountRepository>();
            var duplicateAccount = new Account(1, "Existing", 500);

            // Simulate: input "invalidId" (invalid), "1" (duplicate), "2" (valid)
            var input = new StringReader("invalidId\n1\n2\n");
            var originalIn = Console.In;
            Console.SetIn(input);

            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(duplicateAccount);
            mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Account?)null);

            var service = new AccountService(mockRepo.Object);

            // Act
            await service.CreateAccountAsync(100, "Trifon");

            // Cleanup
            Console.SetIn(originalIn);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
            mockRepo.Verify(r => r.GetByIdAsync(2), Times.Once);
        }

        [Fact]
        public async Task CreateAccountAsync_WithPrompt_ShouldAcceptValidIdFirstTry()
        {
            // Arrange
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(55)).ReturnsAsync((Account?)null);

            var input = new StringReader("55\n");
            var originalIn = Console.In;
            Console.SetIn(input);

            var service = new AccountService(mockRepo.Object);

            // Act
            await service.CreateAccountAsync(555, "Melissa");

            // Cleanup
            Console.SetIn(originalIn);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(55), Times.Once);
        }
    }
}
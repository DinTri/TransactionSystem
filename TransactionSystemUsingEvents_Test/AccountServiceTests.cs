using Microsoft.Extensions.Logging;
using Moq;
using TransactionSystemUsingEvents.Application.EventHandling;
using TransactionSystemUsingEvents.Application.Services;
using TransactionSystemUsingEvents.Domain.Events;
using TransactionSystemUsingEvents.Domain.Interfaces;
using TransactionSystemUsingEvents.Domain.Models;

namespace TransactionSystemUsingEvents_Test
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly Mock<IEventDispatcher> _mockEventDispatcher;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();
            Mock<ILogger<AccountService>> mockLogger = new();
            _mockEventDispatcher = new Mock<IEventDispatcher>();

            _accountService = new AccountService(
                _mockRepository.Object,
                mockLogger.Object,
                _mockEventDispatcher.Object
            );
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldCreateAccountSuccessfully()
        {
            var accountName = "John Doe";
            var initialBalance = 1000m;
            var account = new Account(1, accountName, initialBalance);

            _mockRepository.Setup(r => r.CreateAccountAsync(accountName, initialBalance))
                .ReturnsAsync(account);

            await _accountService.CreateAccountAsync(accountName, initialBalance);

            _mockRepository.Verify(r => r.CreateAccountAsync(accountName, initialBalance), Times.Once);
            _mockEventDispatcher.Verify(d => d.Dispatch(It.IsAny<AccountCreatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task DepositAsync_ShouldDepositSuccessfully()
        {
            var accountId = 1;
            var depositAmount = 200m;
            var account = new Account(accountId, "Trifon Dinev", 5000m);

            _mockRepository.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

            await _accountService.DepositAsync(accountId, depositAmount);

            Assert.Equal(5200m, account.Balance);
            _mockRepository.Verify(r => r.SaveAsync(account), Times.Once);
            _mockEventDispatcher.Verify(d => d.Dispatch(It.IsAny<DepositEvent>()), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldWithdrawSuccessfully()
        {
            var accountId = 1;
            var withdrawAmount = 200m;
            var account = new Account(accountId, "Trifon Dinev", 5000m);

            _mockRepository.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

            await _accountService.WithdrawAsync(accountId, withdrawAmount);

            Assert.Equal(4800m, account.Balance);
            _mockRepository.Verify(r => r.SaveAsync(account), Times.Once);
            _mockEventDispatcher.Verify(d => d.Dispatch(It.IsAny<WithdrawalEvent>()), Times.Once);
        }

        [Fact]
        public async Task TransferAsync_ShouldTransferMoneySuccessfully()
        {
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = 200m;

            var fromAccount = new Account(fromAccountId, "Trifon Dinev", 5000m);
            var toAccount = new Account(toAccountId, "Melissa Thompson", 1000m);

            _mockRepository.Setup(r => r.GetByIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _mockRepository.Setup(r => r.GetByIdAsync(toAccountId)).ReturnsAsync(toAccount);

            await _accountService.TransferAsync(fromAccountId, toAccountId, transferAmount);

            Assert.Equal(4800m, fromAccount.Balance);
            Assert.Equal(1200m, toAccount.Balance);
            _mockRepository.Verify(r => r.SaveAsync(fromAccount), Times.Once);
            _mockRepository.Verify(r => r.SaveAsync(toAccount), Times.Once);
            _mockEventDispatcher.Verify(d => d.Dispatch(It.IsAny<TransferredEvent>()), Times.Once);
        }
    }
}

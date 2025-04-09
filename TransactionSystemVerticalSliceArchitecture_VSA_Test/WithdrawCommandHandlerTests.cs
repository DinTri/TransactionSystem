using FluentAssertions;
using Moq;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Withdraw;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA_Test
{
    public class WithdrawCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly WithdrawHandler _handler;

        public WithdrawCommandHandlerTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _handler = new WithdrawHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldWithdrawAmount()
        {
            // Arrange
            var account = new Account(1, "Trifon Dinev", 5000m);
            var command = new WithdrawCommand(1, 200m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);
            _mockRepo.Setup(repo => repo.SaveAsync(account)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            account.Balance.Should().Be(4800m);
            _mockRepo.Verify(repo => repo.SaveAsync(account), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInsufficientBalance_ShouldThrowException()
        {
            // Arrange
            var account = new Account(1, "Trifon Dinev", 300m);
            var command = new WithdrawCommand(1, 500m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Insufficient balance.", exception.Message);
        }

        [Fact]
        public async Task Handle_GivenNonExistentAccount_ShouldThrowException()
        {
            // Arrange
            var command = new WithdrawCommand(1, 500m);
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

using FluentAssertions;
using Moq;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Deposit;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA_Test
{
    public class DepositCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly DepositHandler _handler;

        public DepositCommandHandlerTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _handler = new DepositHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldDepositAmount()
        {
            // Arrange
            var account = new Account(1, "Trifon Dinev", 5000m);
            var command = new DepositCommand(1, 500m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);
            _mockRepo.Setup(repo => repo.SaveAsync(account)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            account.Balance.Should().Be(5500m);
            _mockRepo.Verify(repo => repo.SaveAsync(account), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenNonExistentAccount_ShouldThrowException()
        {
            // Arrange
            var command = new DepositCommand(1, 500m);
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

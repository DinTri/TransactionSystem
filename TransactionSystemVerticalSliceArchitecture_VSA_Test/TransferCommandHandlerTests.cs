using FluentAssertions;
using Moq;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.Transfer;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA_Test
{
    public class TransferCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly TransferHandler _handler;

        public TransferCommandHandlerTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _handler = new TransferHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldTransferAmount()
        {
            // Arrange
            var fromAccount = new Account(1, "Trifon Dinev", 5000m);
            var toAccount = new Account(2, "Melissa Thompson", 1500m);
            var command = new TransferCommand(1, 2, 300m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(fromAccount);
            _mockRepo.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(toAccount);
            _mockRepo.Setup(repo => repo.SaveAsync(fromAccount)).Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.SaveAsync(toAccount)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            fromAccount.Balance.Should().Be(4700m);
            toAccount.Balance.Should().Be(1800m);
            _mockRepo.Verify(repo => repo.SaveAsync(fromAccount), Times.Once);
            _mockRepo.Verify(repo => repo.SaveAsync(toAccount), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInsufficientFunds_ShouldThrowException()
        {
            // Arrange
            var fromAccount = new Account(1, "Trifon Dinev", 100m);
            var toAccount = new Account(2, "Melissa Thompson", 500m);
            var command = new TransferCommand(1, 2, 300m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(fromAccount);
            _mockRepo.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(toAccount);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenNonExistentAccounts_ShouldThrowException()
        {
            // Arrange
            var command = new TransferCommand(1, 2, 500m);

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Account)null);
            _mockRepo.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

using FluentAssertions;
using Moq;
using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CreateAccount;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories.Interfaces;

namespace TransactionSystemVerticalSliceArchitecture_VSA_Test
{
    public class CreateAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly CreateAccountHandler _handler;

        public CreateAccountCommandHandlerTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _handler = new CreateAccountHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldCreateAccount()
        {
            // Arrange
            var command = new CreateAccountCommand("Trifon Dinev", 5000m);
            var expectedAccount = new Account(1, "Trifon Dinev", 5000m);

            _mockRepo.Setup(repo => repo.CreateAccountAsync(It.IsAny<string>(), It.IsAny<decimal>()))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedAccount);
            _mockRepo.Verify(repo => repo.CreateAccountAsync("Trifon Dinev", 5000m), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidBalance_ShouldThrowException()
        {
            // Arrange
            var command = new CreateAccountCommand("Trifon Dinev", -3000m);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Invalid balance", exception.Message);
        }
    }
}

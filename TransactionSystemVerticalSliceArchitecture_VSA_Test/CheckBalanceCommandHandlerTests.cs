using TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CheckBalance;
using TransactionSystemVerticalSliceArchitecture_VSA.Domain;
using TransactionSystemVerticalSliceArchitecture_VSA.Repositories;

namespace TransactionSystemVerticalSliceArchitecture_VSA_Test
{
    public class CheckBalanceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_AccountExists_ReturnsBalance()
        {
            // Arrange
            var accountId = 1;
            var initialBalance = 100m;
            var repository = new AccountRepository();

            var account = new Account(accountId, "Trifon Dinev", initialBalance);
            await repository.SaveAsync(account);

            var handler = new CheckBalanceCommandHandler(repository);

            // Act
            var command = new CheckBalanceCommand(accountId);
            var balance = await handler.Handle(command, default);

            // Assert
            Assert.Equal(initialBalance, balance);
        }

        [Fact]
        public async Task Handle_AccountDoesNotExist_ThrowsException()
        {
            // Arrange
            var repository = new AccountRepository(); // Simulate an empty repository
            var handler = new CheckBalanceCommandHandler(repository);

            // Act & Assert
            var command = new CheckBalanceCommand(1);
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, default));
        }
    }
}

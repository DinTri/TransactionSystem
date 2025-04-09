using TransactionSystemTheSimplestPossible.Menu;
using TransactionSystemTheSimplestPossible.Repositories;
using TransactionSystemTheSimplestPossible.Services;

namespace TransactionSystemTheSimplestPossible_Test
{
    public class MenuTest
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;

        public MenuTest()
        {
            _accountRepository = new AccountRepository();
            _accountService = new AccountService(_accountRepository);
            new AccountMenu(_accountService);
        }

        [Fact]
        public async Task CreateAccount_ShouldCreateAccountSuccessfully()
        {
            // Arrange
            string owner = "Trifon Dinev";
            decimal initialBalance = 5000m;

            // Act
            var newAccount = await _accountService.CreateAccountAsync(owner, initialBalance);

            // Assert
            Assert.NotNull(newAccount);
            Assert.Equal(owner, newAccount.Owner);
            Assert.Equal(initialBalance, newAccount.Balance);
        }

        [Fact]
        public async Task DepositMoney_ShouldIncreaseBalance()
        {
            // Arrange
            string owner = "Hannah Smith";
            decimal initialBalance = 4000m;
            var account = await _accountService.CreateAccountAsync(owner, initialBalance);
            decimal depositAmount = 500m;

            // Act
            var result = await _accountService.DepositAsync(account.Id, depositAmount);

            // Assert
            Assert.True(result);
            var updatedAccount = await _accountService.GetAccountAsync(account.Id);
            if (updatedAccount != null) Assert.Equal(initialBalance + depositAmount, updatedAccount.Balance);
        }

        [Fact]
        public async Task WithdrawMoney_ShouldDecreaseBalance()
        {
            // Arrange
            string owner = "Hannah Smith";
            decimal initialBalance = 5000m;
            var account = await _accountService.CreateAccountAsync(owner, initialBalance);
            decimal withdrawAmount = 300m;

            // Act
            var result = await _accountService.WithdrawAsync(account.Id, withdrawAmount);

            // Assert
            Assert.True(result);
            var updatedAccount = await _accountService.GetAccountAsync(account.Id);
            Assert.Equal(initialBalance - withdrawAmount, updatedAccount.Balance);
        }

        [Fact]
        public async Task TransferMoney_ShouldTransferFundsBetweenAccounts()
        {
            // Arrange
            string owner1 = "Jane Johnson";
            string owner2 = "Melissa Brown";
            decimal initialBalance = 5000m;

            var account1 = await _accountService.CreateAccountAsync(owner1, initialBalance);
            var account2 = await _accountService.CreateAccountAsync(owner2, initialBalance);
            decimal transferAmount = 500m;

            // Act
            var result = await _accountService.TransferAsync(account1.Id, account2.Id, transferAmount);

            // Assert
            Assert.True(result);
            var updatedAccount1 = await _accountService.GetAccountAsync(account1.Id);
            var updatedAccount2 = await _accountService.GetAccountAsync(account2.Id);
            Assert.Equal(initialBalance - transferAmount, updatedAccount1.Balance);
            Assert.Equal(initialBalance + transferAmount, updatedAccount2.Balance);
        }
    }
}

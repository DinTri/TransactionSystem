using FluentValidation;

namespace TransactionSystemVerticalSliceArchitecture_VSA.Application.Features.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.AccountName).NotEmpty().WithMessage("Account name is required.");
            RuleFor(x => x.InitialBalance).GreaterThan(0).WithMessage("Initial balance must be greater than 0.");
        }
    }
}

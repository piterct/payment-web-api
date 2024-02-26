using FluentValidation;
using Payment.Business.Models;
using Payment.Business.Validations.Documents;

namespace Payment.Business.Validations
{
    public class SellerValidation : AbstractValidator<Seller>
    {
        public SellerValidation()
        {
            RuleFor(f => CpfValidationDocs.Validate(f.Cpf)).Equal(true)
                .WithMessage("The provided document is invalid.");

            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 100)
                .WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must contain only 9 numeric digits.");
        }
    }
}

using FluentValidation;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(r => r.UserName)
                .NotNull()
                .MinimumLength(8).WithMessage("Username must have at least 8 characters");

            RuleFor(r => r.Email)
                .NotNull()
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(r => r.PhoneNo)
                .NotEmpty()
                .Matches(@"^\d{10}$")
                .WithMessage("PhoneNumber must be 10 digits.");

            RuleFor(r => r.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(r => r.Role)
                .Must(r => r.Equals("USER", StringComparison.CurrentCultureIgnoreCase) 
                || r.Equals("ADMIN", StringComparison.CurrentCultureIgnoreCase)
                || r.Equals("GUEST", StringComparison.CurrentCultureIgnoreCase)
                );

            RuleFor(r => r.Password)
                .Must(p => p.Length >= 8
                    && p.Any(char.IsUpper)
                    && p.Any(char.IsLower)
                    && p.Any(char.IsDigit)
                    && p.Any(ch => !char.IsLetterOrDigit(ch)));
        }
    }
}

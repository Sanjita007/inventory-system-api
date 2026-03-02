using FluentValidation;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.Validator
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(r => r.EngName)
                .NotNull();

            RuleFor(r => r.Email)
                .Null()
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(r => r.GroupID)
                .NotNull().WithMessage("Product Group is requred.")
                .GreaterThan(0).WithMessage("Please enter a valid Product Group.");

            RuleFor(r => r.SalesRate)
                .NotEmpty().WithMessage("Sales Rate is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Please enter a valid Sales Rate");

            RuleFor(r=> r.PurchaseRate)
                .NotEmpty().WithMessage("Purchase Rate is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Please enter a valid Purchase Rate");

        }
    }
}

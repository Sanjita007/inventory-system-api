using FluentValidation;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.Validator
{
    public class ProductGroupValidator: AbstractValidator<ProductGroup>
    {
        public ProductGroupValidator()
        {
            RuleFor(r => r.EngName)
                .NotNull();

            RuleFor(r => r.ParentGroupID)
                .NotNull().WithMessage("Parent Group is requred.")
                .GreaterThan(0).WithMessage("Please enter a valid Parent Group.");

            RuleFor(r => r.ParentGroupID)
               .NotEqual(r => r.ID).WithMessage("Parent Group cannot be the same as the current group.");



        }
    }
}

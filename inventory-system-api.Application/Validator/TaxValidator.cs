using FluentValidation;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Application.Validator
{
    public class TaxValidator : AbstractValidator<Tax>
    {
        public TaxValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Tax name is required.");

            RuleFor(r => r.Code)
                .NotEmpty().WithMessage("Tax code is required.");

            RuleFor(r => r.Rate)
                .GreaterThan(0).WithMessage("Tax Rate must be greater than 0.");


        }
    }
}

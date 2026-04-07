using FluentValidation;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.Validator
{
    public class CompoundUnitValidator: AbstractValidator<CompoundUnit>
    {
        public CompoundUnitValidator()
        {
            RuleFor(r => r.UnitID)
                .NotNull().WithMessage("Unit is required");

            RuleFor(r => r.RelationValue)
               .NotNull().WithMessage("Relation value is required")
               .GreaterThan(0).WithMessage("Relation value must be greater than 0");

            RuleFor(r => r.ParentUnitID)
               .NotEqual(r => r.UnitID).WithMessage("Unit and relation unit cannot be the same");
        }
    }
}

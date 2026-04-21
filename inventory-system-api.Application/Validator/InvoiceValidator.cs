using FluentValidation;
using inventory_system_api.Application.Models.Inventory;

namespace inventory_system_api.Application.Validator
{
    public class InvoiceValidator : AbstractValidator<InvoiceMaster>
    {
        public InvoiceValidator()
        {
            RuleFor(r => r.VoucherNo)
                .NotNull().WithMessage("Voucher Number is required.");

            RuleFor(r => r.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(r => r.TotalQty)
                .GreaterThan(0).WithMessage("Total quantity must be greater than 0.");

        }
    }

    public class InvoiceDetailValidator : AbstractValidator<InvoiceDetail>
    {
        public InvoiceDetailValidator()
        {
            RuleFor(r => r.ProductID)
                .NotNull().WithMessage("Product is required.");

            RuleFor(r => r.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be less than 0.");

            RuleFor(r => r.Quantity)
                .GreaterThan(0).WithMessage("Total quantity must be greater than 0.");


        }
    }
}

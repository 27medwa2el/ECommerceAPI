using FluentValidation;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID is required");

            RuleFor(x => x.Products)
                .NotEmpty().WithMessage("Order must contain at least one product")
                .Must(products => products.Count > 0).WithMessage("Order must contain at least one product");

            RuleForEach(x => x.Products).SetValidator(new OrderProductValidator());
        }
    }

    public class OrderProductValidator : AbstractValidator<OrderProductDto>
    {
        public OrderProductValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be valid");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }

    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusDto>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => status == "Pending" || status == "Delivered")
                .WithMessage("Status must be either 'Pending' or 'Delivered'");
        }
    }
}

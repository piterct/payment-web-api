using FluentValidation;
using Payment.Business.Models;

namespace Payment.Business.Validations
{
    public class OrderValidation : AbstractValidator<Order>
    {
        public OrderValidation()
        {
            RuleFor(f => f.OrderItems.Count)
                .NotEqual(0).WithMessage("A Order must have at least 1 item");

            RuleFor(f => f.SellerId)
                .NotEqual(Guid.Empty).WithMessage("It's necessary to have a seller for the order.");
        }
    }
}

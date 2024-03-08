using System.Data.SqlTypes;
using FluentValidation;
using Payment.Business.Enums;
using Payment.Business.Models;
using Payment.Business.Validations.Documents;

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

        public OrderValidation UpdateStatusOrder(EOrderStatus eOrderStatus)
        {
            RuleFor(f => f.IsStatusUpdateAllowed(eOrderStatus)).Equal(true)
                .WithMessage(x =>
                    string.Concat("It's not allowed to update the order status from ", x.Status.ToString(), " to ", eOrderStatus.ToString()));

            return this;
        }
    }
}

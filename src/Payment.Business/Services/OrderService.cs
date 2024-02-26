using Payment.Business.Dtos;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Models;
using Payment.Business.Validations;
using static Payment.Business.Models.Order;

namespace Payment.Business.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderQuery _orderQuery;

        public OrderService(INotifier notifier, IOrderRepository orderRepository, IOrderQuery orderQuery) : base(notifier)
        {
            _orderRepository = orderRepository;
            _orderQuery = orderQuery;
        }

        public async Task<NewOrderDto> Add(Order order)
        {
            var newOrder = OrderFactory.NewOrder(order.SellerId);

            newOrder.AddOrderItems(order._orderItems);

            if (!RunValidation(new OrderValidation(), newOrder)) return new NewOrderDto();

            await _orderRepository.Add(newOrder);

            return new NewOrderDto(newOrder.Id);
        }

        public async Task<OrderDto> UpdateOrderStatus(Order order, EOrderStatus newStatus)
        {
            if (IsStatusUpdateAllowed(order.Status, newStatus))
            {
                order.UpdateOrderStatus(newStatus);
                await _orderRepository.Update(order);
                return await _orderQuery.GetById(order.Id);
            }
            else
            {
                Notify(string.Concat("It's not allowed to update the order status from ", order.Status.ToString(), " to ", newStatus.ToString()));
                return new OrderDto();
            }
        }

        private bool IsStatusUpdateAllowed(EOrderStatus currentStatus, EOrderStatus newStatus)
        {
            switch (currentStatus)
            {
                case EOrderStatus.AwaitingPayment:
                    return IsStatusAwaitingPaymentUpdateAllowed(newStatus);
                case EOrderStatus.Approved:
                    return IsStatusApprovedUpdateAllowed(newStatus);
                case EOrderStatus.Shipped:
                    return IsStatusShippedAllowed(newStatus);
                default:
                    return false;
            }
        }

        private bool IsStatusAwaitingPaymentUpdateAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Approved || newStatus == EOrderStatus.Cancelled)
                return true;

            return false;
        }

        private bool IsStatusApprovedUpdateAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Shipped || newStatus == EOrderStatus.Cancelled)
                return true;

            return false;
        }

        private bool IsStatusShippedAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Delivered)
                return true;

            return false;
        }
    }
}

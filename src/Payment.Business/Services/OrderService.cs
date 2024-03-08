using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderService(INotifier notifier, IOrderRepository orderRepository, IOrderQuery orderQuery, IMapper mapper) : base(notifier)
        {
            _orderRepository = orderRepository;
            _orderQuery = orderQuery;
            _mapper = mapper;
        }

        public async Task<NewOrderDto> Add(OrderDto order)
        {
            var newOrder = OrderFactory.NewOrder(order.Seller.Id);

            var items = _mapper.Map<List<OrderItem>>(order.Items);

            newOrder.AddOrderItems(items);

            if (!RunValidation(new OrderValidation(), newOrder)) return new NewOrderDto();

            await _orderRepository.Add(newOrder);

            await _orderRepository.UnitOfWork.Commit();

            return new NewOrderDto(newOrder.Id);
        }

        public async Task<OrderDto> UpdateOrderStatus(Order order, EOrderStatus newStatus)
        {
            if (!RunValidation(new OrderValidation().UpdateStatusOrder(newStatus), order)) return new OrderDto();

            order.UpdateOrderStatus(newStatus);
            await _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.Commit();
            return await _orderQuery.GetById(order.Id);

        }

    }
}

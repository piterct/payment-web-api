using AutoMapper;
using Payment.Business.Dtos;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Interfaces.Repositories;

namespace Payment.Business.Queries
{
    public class OrderQuery : IOrderQuery
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ISellerRepository _sellerRepository;

        public OrderQuery(IMapper mapper, IOrderRepository orderRepository, ISellerRepository sellerRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _sellerRepository = sellerRepository;
        }
        public async Task<OrderDto> GetById(Guid orderId)
        {
            var order = await _orderRepository.GetAllItemsById(orderId);
            if (order is not null)
            {
                var seller = await _sellerRepository.GetById(order.SellerId);

                var orderItemDto = _mapper.Map<List<OrderItemDto>>(order.OrderItems);

                return new OrderDto(order.Id, order.Status.ToString(), order.Date, orderItemDto, seller);
            }

            return new OrderDto();
        }
    }
}

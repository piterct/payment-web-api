using AutoMapper;
using Moq;
using Moq.AutoMock;
using Payment.Business.Dtos;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Business.Services;

namespace Payment.Tests.Services.Order
{
    public class AddOrderServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderService _orderService;
        private readonly Guid _orderId;
        private readonly List<OrderItemDto> _emptyListOrderItemDto;
        private readonly Business.Models.Seller _emptySeller;
        private readonly List<OrderItem> _listOrderItem;

        public AddOrderServiceTests()
        {
            _mocker = new AutoMocker();
            _orderService = _mocker.CreateInstance<OrderService>();
            _orderId = Guid.NewGuid();
            _emptyListOrderItemDto = new List<OrderItemDto>();
            _emptySeller = new Business.Models.Seller();
            _listOrderItem = new List<OrderItem>();
        }

        [Fact(DisplayName = "Add new valid order")]
        [Trait("Category", "Order Services")]
        public async void Order_NewValidOrder_MustCreateOrder()
        {
            // Arrange
            var orderItem = new OrderItem("Bike");
            _listOrderItem.Add(orderItem);
            var orderDto = new OrderDto(_orderId, EOrderStatus.Cancelled.ToString(), DateTime.Now, _emptyListOrderItemDto, _emptySeller);
            _mocker.GetMock<IMapper>().Setup(m =>
                m.Map<List<OrderItem>>(It.IsAny<List<OrderItemDto>>())).Returns(_listOrderItem);

            // Act
            var orderService = await _orderService.Add(orderDto);

            // Assert 
            Assert.NotEqual(orderService.OrderId, orderDto.OrderId);
            _mocker.GetMock<IOrderRepository>().Verify(s => s.Add(It.IsAny<Business.Models.Order>()), Times.Once());
        }

        [Fact(DisplayName = "Add new invalid order without items")]
        [Trait("Category", "Order Services")]
        public async void Order_NewInvalidOrderWithoutItems_MustNotCreateOrder()
        {
            // Arrange
            var orderDto = new OrderDto(_orderId, EOrderStatus.Cancelled.ToString(), DateTime.Now, _emptyListOrderItemDto, _emptySeller);
            _mocker.GetMock<IMapper>().Setup(m =>
                m.Map<List<OrderItem>>(It.IsAny<List<OrderItemDto>>())).Returns(_listOrderItem);

            //Act
            var orderService = await _orderService.Add(orderDto);

            // Assert 
            Assert.NotEqual(orderService.OrderId, orderDto.OrderId);
            Assert.Equal(Guid.Empty, orderService.OrderId);
            _mocker.GetMock<IOrderRepository>().Verify(s => s.Add(It.IsAny<Business.Models.Order>()), Times.Never());
        }

    }

}

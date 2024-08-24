using Moq;
using Moq.AutoMock;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Business.Queries;

namespace Payment.Tests.Queries.Order
{
    public class GetByIdOrderQueryTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderQuery _orderQuery;
        private readonly Guid _orderId;

        public GetByIdOrderQueryTests()
        {
            _mocker = new AutoMocker();
            _orderQuery = _mocker.CreateInstance<OrderQuery>();
            _orderId = Guid.NewGuid();
        }

        [Fact(DisplayName = "Get order by id")]
        [Trait("Category", "Order Queries")]
        public async void Order_GetOrderById_MustReturnAOrder()
        {
            // Arrange
            var order = Business.Models.Order.OrderFactory.NewOrder(_orderId);
            var orderItem = new OrderItem("Bike");
            Guid orderIdCalled = Guid.Empty;
            orderItem.AssociateOrder(order.Id);
            _mocker.GetMock<IOrderRepository>().Setup(query => query.GetAllItemsById(_orderId))
                .Callback<Guid>((orderId) =>
                {
                    orderIdCalled = orderId;
                })
                .ReturnsAsync(await Task.FromResult(order));

            // Act
            var newOrder = await _orderQuery.GetById(_orderId);

            // Assert 
            _mocker.Verify();
            _mocker.GetMock<IOrderRepository>().Verify(r => r.GetAllItemsById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<ISellerRepository>().Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.Equal(_orderId, orderIdCalled);

        }

        [Fact(DisplayName = "Order not found by Id")]
        [Trait("Category", "Order Queries")]
        public async void Order_GetOrderById_MustNotReturnACompleteOrder()
        {
            // Act
            await _orderQuery.GetById(_orderId);

            // Assert 
            _mocker.Verify();
            _mocker.GetMock<IOrderRepository>().Verify(r => r.GetAllItemsById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<ISellerRepository>().Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never());
        }

    }

}

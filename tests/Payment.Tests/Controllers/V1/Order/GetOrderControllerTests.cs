using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Payment.Api.Controllers.V1;
using Payment.Business.Dtos;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Models;
using Payment.Business.Notifications;

namespace Payment.Tests.Controllers.V1.Order
{
    public class GetOrderControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderController _orderController;
        private readonly Guid _orderId;
        private readonly Guid _emptyOrderId;

        public GetOrderControllerTests()
        {

            _mocker = new AutoMocker();
            _orderController = _mocker.CreateInstance<OrderController>();
            _orderId = Guid.NewGuid();
            _emptyOrderId = Guid.Empty;
        }

        [Fact(DisplayName = "Get order by id")]
        [Trait("Category", "Order Controllers")]
        public async void Order_GetOrderById_MustReturnAOrder()
        {
            // Arrange
            var order = Business.Models.Order.OrderFactory.NewOrder(_orderId);
            var orderItem = new OrderItem("Bike");
            orderItem.AssociateOrder(order.Id);
            var orderDto = new OrderDto(order.Id, order.Status.ToString(), order.Date, new List<OrderItemDto>(), new Business.Models.Seller());
            _mocker.GetMock<IOrderQuery>().Setup(query => query.GetById(_orderId)).ReturnsAsync(await Task.FromResult(orderDto));

            // Act
            var result = await _orderController.GetById(_orderId);

            // Assert 
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, statusCodeResult.StatusCode);
            _mocker.GetMock<IOrderQuery>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
        }

        [Fact(DisplayName = "Get order by id not found")]
        [Trait("Category", "Order Controllers")]
        public async void Order_GetOrderById_MustReturnNotFound()
        {
            // Arrange
            var orderDto = new OrderDto(_emptyOrderId, EOrderStatus.AwaitingPayment.ToString(), DateTime.Now, new List<OrderItemDto>(), new Business.Models.Seller());
            _mocker.GetMock<IOrderQuery>().Setup(query => query.GetById(_orderId)).ReturnsAsync(await Task.FromResult(orderDto));
            _mocker.GetMock<INotifier>().Setup(n => n.GetNotifications())
                .Returns(new List<Notification>() { new Notification("Order not found") });

            // Act
            var result = await _orderController.GetById(_orderId);

            // Assert 
            var statusCodeResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
            _mocker.GetMock<IOrderQuery>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
        }
    }
}

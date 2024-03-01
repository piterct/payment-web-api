using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Payment.Api.Controllers.V1;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Models;
using Payment.Business.Notifications;

namespace Payment.Tests.Controllers.V1.Order
{
    public class UpdateStatusOrderControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderController _orderController;
        private readonly Guid _orderId;

        public UpdateStatusOrderControllerTests()
        {
            _mocker = new AutoMocker();
            _orderController = _mocker.CreateInstance<OrderController>();
            _orderId = Guid.NewGuid();
        }

        [Fact(DisplayName = "Update valid order")]
        [Trait("Category", "Order Controllers")]
        public async void Order_UpdateOrder_MustReturnAOrder()
        {
            // Arrange
            var order = Business.Models.Order.OrderFactory.NewOrder(_orderId);
            var orderItem = new OrderItem("Bike");
            orderItem.AssociateOrder(order.Id);
            order.AddOrderItems(orderItem);
            _mocker.GetMock<IOrderRepository>().Setup(r => r.GetById(_orderId)).ReturnsAsync(await Task.FromResult(order));

            // Act
            var result = await _orderController.UpdateOrderStatus(_orderId, EOrderStatus.Approved);

            // Assert 
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, statusCodeResult.StatusCode);
            _mocker.GetMock<IOrderRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderService>().Verify(s => s.UpdateOrderStatus(It.IsAny<Business.Models.Order>(), EOrderStatus.Approved), Times.Once());
        }

        [Fact(DisplayName = "Update order not found")]
        [Trait("Category", "Order Controllers")]
        public async void Order_OrderNotFound_MustReturnNotFound()
        {
            // Arrange
            _mocker.GetMock<INotifier>().Setup(n => n.GetNotifications())
                .Returns(new List<Notification>() { new Notification("Order not found") });

            // Act
            var result = await _orderController.UpdateOrderStatus(_orderId, EOrderStatus.Approved);

            // Assert 
            var statusCodeResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
            _mocker.GetMock<IOrderRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderService>().Verify(s => s.UpdateOrderStatus(It.IsAny<Business.Models.Order>(), EOrderStatus.Approved), Times.Never());
        }

        [Fact(DisplayName = "Updating order status not allowed")]
        [Trait("Category", "Order Controllers")]
        public async void Order_UpdatingOrderStatusNotAllowed_MustReturnBadRequest()
        {
            // Arrange
            var order = Business.Models.Order.OrderFactory.NewOrder(_orderId);
            var stringMessage = string.Concat("It's not allowed to update the order status from ", order.Status.ToString(), " to ", EOrderStatus.Shipped.ToString());
            var orderItem = new OrderItem("Bike");
            orderItem.AssociateOrder(order.Id);
            order.AddOrderItems(orderItem);
            _mocker.GetMock<IOrderRepository>().Setup(r => r.GetById(_orderId)).ReturnsAsync(await Task.FromResult(order));
            _mocker.GetMock<INotifier>().Setup(n => n.HasNotification()).Returns(true);
            _mocker.GetMock<INotifier>().Setup(n => n.GetNotifications()).Returns(new List<Notification>() { new Notification(stringMessage) });

            // Act
            var result = await _orderController.UpdateOrderStatus(_orderId, EOrderStatus.Shipped);

            // Assert 
            var statusCodeResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, statusCodeResult.StatusCode);
            _mocker.GetMock<IOrderRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderService>().Verify(s => s.UpdateOrderStatus(It.IsAny<Business.Models.Order>(), EOrderStatus.Shipped), Times.Once());
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Payment.Api.Controllers.V1;
using Payment.Api.ViewModels;
using Payment.Business.Dtos;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Notifications;

namespace Payment.Tests.Controllers.V1.Order
{
    public class AddOrderControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderController _orderController;
        private readonly Guid _orderId;
        private readonly Guid _emptyOrderId;
        private readonly Guid _sellerId;
        private readonly Business.Models.Seller _seller;
        private readonly List<ValidationResult> _validationResults;

        public AddOrderControllerTests()
        {
            _mocker = new AutoMocker();
            _orderController = _mocker.CreateInstance<OrderController>();
            _orderId = Guid.NewGuid();
            _emptyOrderId = Guid.Empty;
            _sellerId = Guid.NewGuid();
            _seller = new Business.Models.Seller(_sellerId, "042.593.100-54", "Jhon Perez", "tests.tests@gmail.com", "123456789");
            _validationResults = new List<ValidationResult>();
        }

        [Fact(DisplayName = "Add valid order")]
        [Trait("Category", "Order Controllers")]
        public async void AddOrder_AddValidOrder_MustCreateOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest(_sellerId.ToString(), new List<OrderItemRequest>());
            _mocker.GetMock<ISellerRepository>().Setup(m => m.GetById(_sellerId)).ReturnsAsync(_seller);

            // Act
            var result = await _orderController.AddOrder(orderRequest);
            var isValid = Validator.TryValidateObject(orderRequest, new ValidationContext(orderRequest), _validationResults, true);

            // Assert 
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(isValid);
            Assert.Equal(200, statusCodeResult.StatusCode);
            _mocker.GetMock<ISellerRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderService>().Verify(s => s.Add(It.IsAny<OrderDto>()), Times.Once());
        }

        [Fact(DisplayName = "Add order seller not found ")]
        [Trait("Category", "Order Controllers")]
        public async void AddOrder_SellerNotFound_MustReturnNotFound()
        {
            // Arrange
            var orderRequest = new OrderRequest(_sellerId.ToString(), new List<OrderItemRequest>());
            _mocker.GetMock<INotifier>().Setup(n => n.GetNotifications())
                .Returns(new List<Notification>() { new Notification("Seller not found") });

            // Act
            var result = await _orderController.AddOrder(orderRequest);
            var isValid = Validator.TryValidateObject(orderRequest, new ValidationContext(orderRequest), _validationResults, true);

            // Assert 
            var statusCodeResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(isValid);
            Assert.Equal(404, statusCodeResult.StatusCode);
            _mocker.GetMock<ISellerRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderService>().Verify(s => s.Add(It.IsAny<OrderDto>()), Times.Never());
        }
    }
}

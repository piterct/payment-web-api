using Moq;
using Moq.AutoMock;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Data;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Business.Services;

namespace Payment.Tests.Services.Order
{
    public class UpdateStatusOrderServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderService _orderService;
        private readonly Guid _orderId;
        private readonly Business.Models.Order _order;

        public UpdateStatusOrderServiceTests()
        {
            _mocker = new AutoMocker();
            _orderService = _mocker.CreateInstance<OrderService>();
            _orderId = Guid.NewGuid();
            _order = Business.Models.Order.OrderFactory.NewOrder(_orderId);
            var orderItem = new OrderItem("Bike");
            orderItem.AssociateOrder(_order.Id);
            _order.AddOrderItems(orderItem);
        }

        #region Status AwaitingPayment

        [Fact(DisplayName = "Status change from AwaitingPayment to Approved allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusAwaitingPaymentToApproved_MustUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Approved);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Once());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderRepository>().Verify(u => u.UnitOfWork.Commit(), Times.Once());
        }

        [Fact(DisplayName = "Status change from AwaitingPayment to Cancelled allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusAwaitingPaymentToCancelled_MustUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Cancelled);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Once());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderRepository>().Verify(u => u.UnitOfWork.Commit(), Times.Once());
        }

        [Fact(DisplayName = "Status change from AwaitingPayment to Shipped not allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusAwaitingPaymentToShipped_MustNotUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(false);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Shipped);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Never());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Never());
            _mocker.GetMock<IOrderRepository>().Verify(u => u.UnitOfWork.Commit(), Times.Never());
        }

        #endregion

        #region Status Approved

        [Fact(DisplayName = "Status change from Approved to Shipped allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusApprovedToShipped_MustUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
            _order.UpdateOrderStatus(EOrderStatus.Approved);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Shipped);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Once());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Once());
        }


        [Fact(DisplayName = "Status change from Approved to Cancelled allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusApprovedToCancelled_MustUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
            _order.UpdateOrderStatus(EOrderStatus.Approved);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Cancelled);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Once());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Once());
        }

        [Fact(DisplayName = "Status change from Approved to AwaitingPayment not allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusApprovedToAwaitingPayment_MustNotUpdate()
        {
            //Arrange
            _order.UpdateOrderStatus(EOrderStatus.Approved);

            // Act
            var orderService = await _orderService.UpdateOrderStatus(_order, EOrderStatus.AwaitingPayment);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Never());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Never());
        }


        #endregion

        #region Status Shipped

        [Fact(DisplayName = "Status change from Shipped to Delivered allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusShippedToDelivered_MustUpdate()
        {
            //Arrange
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
            _order.UpdateOrderStatus(EOrderStatus.Shipped);
           

            // Act
            var orderService = await _orderService.UpdateOrderStatus(_order, EOrderStatus.Delivered);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Once());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once());
        }

        [Fact(DisplayName = "Status change from Shipped to Approved not allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusShippedToApproved_MustUpdate()
        {
            //Arrange
            _order.UpdateOrderStatus(EOrderStatus.Shipped);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Approved);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Never());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Never());
        }
        #endregion

        #region Status Cancelled

        [Fact(DisplayName = "Status change from Cancelled to Approved not allowed")]
        [Trait("Category", "Order Services")]
        public async void Order_UpdatingStatusCancelledToApproved_MustNotUpdate()
        {
            //Arrange
            _order.UpdateOrderStatus(EOrderStatus.Cancelled);

            // Act
            await _orderService.UpdateOrderStatus(_order, EOrderStatus.Approved);

            // Assert 
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Business.Models.Order>()), Times.Never());
            _mocker.GetMock<IOrderQuery>().Verify(q => q.GetById(It.IsAny<Guid>()), Times.Never());
        }

        #endregion

    }
}

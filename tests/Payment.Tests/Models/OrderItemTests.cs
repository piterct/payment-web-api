using Payment.Business.Models;

namespace Payment.Tests.Models
{
    public  class OrderItemTests
    {
        [Fact(DisplayName = "Create valid order item")]
        public void OrderItem_ValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            const string name = "Product A";
            
            // Act
            var orderItem = new OrderItem(name);
            
            // Assert
            Assert.Equal(name, orderItem.Name);
        }

        [Fact(DisplayName = "Associate valid order")]
        public void AssociateOrder_ValidOrder_ShouldAssociateSuccessfully()
        {
            // Arrange
            var orderItem = new OrderItem("Product A");
            var orderId = Guid.NewGuid();
            
            // Act
            orderItem.AssociateOrder(orderId);
            
            // Assert
            Assert.Equal(orderId, orderItem.OrderId);
        }

        [Fact(DisplayName = "Replace existing order association")]
        public void AssociateOrder_ReplaceExistingOrder_ShouldUpdateOrderId()
        {
            // Arrange
            var orderItem = new OrderItem("Product A");
            var initialOrderId = Guid.NewGuid();
            var newOrderId = Guid.NewGuid();
            orderItem.AssociateOrder(initialOrderId);
            
            // Act
            orderItem.AssociateOrder(newOrderId);
            
            // Assert
            Assert.Equal(newOrderId, orderItem.OrderId);
        }
    }
}

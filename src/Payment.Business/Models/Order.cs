using Payment.Business.Enums;

namespace Payment.Business.Models
{
    public class Order : Entity
    {
        public Guid SellerId { get; private set; }
        public List<OrderItem> _orderItems { get; private set; }
        public EOrderStatus Status { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public DateTime Date { get; private set; }

        public Order()
        {
            _orderItems = new List<OrderItem>();
        }
        public void SetAwaitingPayment()
        {
            Status = EOrderStatus.AwaitingPayment;
        }

        public void UpdateOrderStatus(EOrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void AddOrderItems(List<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                _orderItems.Add(item);
                item.AssociateOrder(Id);
            }
        }

        public void AddOrderItems(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
        }

        public static class OrderFactory
        {
            public static Order NewOrder(Guid sellerId)
            {
                var order = new Order
                {
                    SellerId = sellerId,
                    Date = DateTime.Now,

                };

                order.SetAwaitingPayment();
                return order;
            }
        }
    }
}

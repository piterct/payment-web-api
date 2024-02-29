using Payment.Business.Enums;

namespace Payment.Business.Models
{
    public class Order : Entity
    {
        public Guid SellerId { get; private set; }
        public EOrderStatus Status { get; private set; }
        public DateTime Date { get; private set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

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

        public bool IsStatusUpdateAllowed(EOrderStatus newStatus)
        {
            switch (Status)
            {
                case EOrderStatus.AwaitingPayment:
                    return IsStatusAwaitingPaymentUpdateAllowed(newStatus);
                case EOrderStatus.Approved:
                    return IsStatusApprovedUpdateAllowed(newStatus);
                case EOrderStatus.Shipped:
                    return IsStatusShippedAllowed(newStatus);
                default:
                    return false;
            }
        }

        public static bool IsStatusAwaitingPaymentUpdateAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Approved || newStatus == EOrderStatus.Cancelled)
                return true;

            return false;
        }

        public static bool IsStatusApprovedUpdateAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Shipped || newStatus == EOrderStatus.Cancelled)
                return true;

            return false;
        }

        public static bool IsStatusShippedAllowed(EOrderStatus newStatus)
        {
            if (newStatus == EOrderStatus.Delivered)
                return true;

            return false;
        }
    }
}

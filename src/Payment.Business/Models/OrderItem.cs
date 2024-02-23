namespace Payment.Business.Models
{
    public class OrderItem : Entity
    {
        public OrderItem(string name)
        {
            Name = name;
        }

        public OrderItem() { }
        public Guid OrderId { get; private set; }
        public string Name { get; private set; }

        public Order Order { get; private set; }

        public void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}

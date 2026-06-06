namespace Payment.Business.Models
{
    public class OrderItem : Entity
    {
        public OrderItem(string name)
        {
            Name = name;
        }

        public OrderItem()
        { }
        
        public Guid OrderId { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;

        public Order Order { get; private set; } = new();

        public void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}

namespace Payment.Business.Dtos
{
    public class NewOrderDto
    {
        public NewOrderDto(Guid orderId)
        {
            OrderId = orderId;
        }

        public NewOrderDto()
        { }
        public Guid OrderId { get; set; }
    }
}

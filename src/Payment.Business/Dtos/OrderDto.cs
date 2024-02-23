using Payment.Business.Models;

namespace Payment.Business.Dtos
{
    public record  OrderDto
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public Seller Seller { get; set; }

        public OrderDto() { }
        public OrderDto(Guid orderId, string status, DateTime date, List<OrderItemDto> items, Seller seller)
        {
            OrderId = orderId;
            Status = status;
            Date = date;
            Items = items;
            Seller = seller;
        }
    }
}

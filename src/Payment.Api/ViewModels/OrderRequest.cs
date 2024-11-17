using Payment.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModels
{
    public class OrderRequest
    {
        public OrderRequest(string sellerId, List<OrderItemRequest> items)
        {
            SellerId = sellerId;
            Items = items;
        }
        [GuidValidation(ErrorMessage = "Invalid GUID format.")]
        [Required(ErrorMessage = "The {0} field is mandatory")]
        public string SellerId { get; set; }

        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}

using Payment.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModels
{
    public class OrderRequest
    {
        public OrderRequest(string sellerID, List<OrderItemRequest> items)
        {
            SellerId = sellerID;
            Items = items;
        }
        [GuidValidation(ErrorMessage = "Invalid GUID format.")]
        [Required(ErrorMessage = "The {0} field is mandatory")]
        public string SellerId { get; set; }

        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}

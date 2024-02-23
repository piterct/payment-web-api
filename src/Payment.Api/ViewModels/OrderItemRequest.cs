using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModels
{
    public class OrderItemRequest
    {
        [Required(ErrorMessage = "The {0} field is mandatory")]
        [StringLength(60, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }
    }
}

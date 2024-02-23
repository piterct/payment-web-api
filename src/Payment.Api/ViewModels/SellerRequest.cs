using Payment.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModels
{
    public class SellerRequest
    {
        public SellerRequest(string id, string cpf, string name, string email, string phone)
        {
            Id = id;
            Cpf = cpf;
            Name = name;
            Email = email;
            Phone = phone;
        }
        public SellerRequest() { }

        [GuidValidation(ErrorMessage = "Invalid GUID format.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "The {0} field is mandatory")]
        [StringLength(16, MinimumLength = 11, ErrorMessage = "The {0} field must have {1} characteres.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "The {0} field is mandatory")]
        [StringLength(60, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is mandatory")]
        [EmailAddress(ErrorMessage = "The field {0} is in an invalid format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The {0} field is mandatory")]
        [StringLength(9, ErrorMessage = "The {0} field must have {1} characteres.", MinimumLength = 9)]
        public string Phone { get; set; }
    }
}

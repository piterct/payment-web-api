using System.ComponentModel.DataAnnotations;

namespace Payment.Api.Attributes
{
    public class GuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Treat null or empty strings as valid to allow for other validation attributes
                return new ValidationResult(ErrorMessage ?? "Invalid GUID format.");
            }

            string? guidString = value?.ToString();


            if (!Guid.TryParse(guidString, out _) || Guid.Empty.ToString() == guidString)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid GUID format.");
            }


            return ValidationResult.Success;
        }
    }
}

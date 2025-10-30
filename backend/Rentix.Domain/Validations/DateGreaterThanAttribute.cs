using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Validations
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public DateGreaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
            ErrorMessage = "The end date must be greater than the start date.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if decorated attribute is valid
            if (value == null)
                return ValidationResult.Success;

            // Get the other date to compare
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherValue == null)
                return ValidationResult.Success;

            if (value is DateTime thisDate && otherValue is DateTime otherDate)
            {
                if (thisDate <= otherDate)
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

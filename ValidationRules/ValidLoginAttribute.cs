using System.ComponentModel.DataAnnotations;

namespace ValidationRules
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidLoginAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var login = value as string;

            if (string.IsNullOrWhiteSpace(login))
                return new ValidationResult("Логин должен быть заполнен");

            return ValidationResult.Success!;
        }
    }
}
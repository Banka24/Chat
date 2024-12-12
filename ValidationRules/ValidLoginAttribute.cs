using System.ComponentModel.DataAnnotations;

namespace Chat.ClientApp.ValidationRules
{
    /// <summary>
/// Пользовательский атрибут, который может быть применен к свойствам в классах, чтобы указать, что значение этого свойства должно быть валидным логином.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidLoginAttribute : ValidationAttribute
{
    /// <summary>
    /// Проверяет, что значение свойства не является пустым или пробелами.
    /// </summary>
    /// <param name="value">Значение свойства.</param>
    /// <param name="validationContext">Контекст валидации.</param>
    /// <returns>Результат валидации.</returns>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var login = value as string;

        if (string.IsNullOrWhiteSpace(login))
            return new ValidationResult("Логин должен быть заполнен");

        return ValidationResult.Success!;
    }
}

}
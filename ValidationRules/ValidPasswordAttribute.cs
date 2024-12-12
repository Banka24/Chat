using System.ComponentModel.DataAnnotations;

namespace Chat.ClientApp.ValidationRules
{
    /// <summary>
/// Пользовательский атрибут, который может быть применен к свойствам в классах, чтобы указать, что значение этого свойства должно быть валидным паролем.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidPasswordAttribute : ValidationAttribute
{
    /// <summary>
    /// Проверяет, что значение свойства является валидным паролем.
    /// </summary>
    /// <param name="value">Значение свойства.</param>
    /// <param name="validationContext">Контекст валидации.</param>
    /// <returns>Результат валидации.</returns>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return new ValidationResult("Пароль должен содержать как минимум 8 символов.");

        if (!password.Any(char.IsDigit))
            return new ValidationResult("Пароль должен содержать хотя бы одну цифру.");

        if (!password.Any(char.IsUpper))
            return new ValidationResult("Пароль должен содержать хотя бы одну заглавную букву.");

        if (!password.Any(char.IsLower))
            return new ValidationResult("Пароль должен содержать хотя бы одну строчную букву.");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return new ValidationResult("Пароль должен содержать хотя бы один специальный символ.");

        return ValidationResult.Success!;
    }
}

}
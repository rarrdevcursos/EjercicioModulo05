using System.ComponentModel.DataAnnotations;

namespace admEventEase.Models;

public sealed class EventRegistrationModel
{
    [Required(ErrorMessage = "Escribe tu nombre.")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres.")]
    [CustomValidation(typeof(EventRegistrationModel), nameof(ValidateName))]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu correo electrónico.")]
    [EmailAddress(ErrorMessage = "Escribe un correo electrónico válido.")]
    [StringLength(254, ErrorMessage = "El correo electrónico no puede superar los 254 caracteres.")]
    public string Email { get; set; } = string.Empty;

    public static ValidationResult? ValidateName(object? value, ValidationContext _)
    {
        var name = value as string;
        return name is not null && name.Trim().Length >= 2
            ? ValidationResult.Success
            : new ValidationResult("El nombre debe tener al menos 2 caracteres.");
    }
}

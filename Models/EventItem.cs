using System.ComponentModel.DataAnnotations;

namespace admEventEase.Models;

public sealed class EventItem
{
    [Range(1, int.MaxValue)]
    public int Id { get; init; }

    [Required]
    [StringLength(120)]
    public string Name { get; init; } = string.Empty;

    [CustomValidation(typeof(EventItem), nameof(ValidateDate))]
    public DateTime Date { get; init; }

    [Required]
    [StringLength(200)]
    public string Location { get; init; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; init; } = string.Empty;

    public bool IsValid
    {
        get
        {
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, new ValidationContext(this), results, validateAllProperties: true);
        }
    }

    public static ValidationResult? ValidateDate(object? value, ValidationContext _)
    {
        return value is DateTime date && date != default
            ? ValidationResult.Success
            : new ValidationResult("La fecha del evento es obligatoria.");
    }
}

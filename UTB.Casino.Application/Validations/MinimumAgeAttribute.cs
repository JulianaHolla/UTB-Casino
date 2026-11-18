using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.Validations;

// Own validation attribute: the date of birth must be at least N years ago.
// Client side part is in the Web project (Validations/Adapters).
[AttributeUsage(AttributeTargets.Property)]
public class MinimumAgeAttribute : ValidationAttribute
{
    public int Years { get; }

    public MinimumAgeAttribute(int years)
    {
        Years = years;
        ErrorMessage = $"You must be at least {years} years old.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime dob) return ValidationResult.Success;   // [Required] handles empty values
        return AgeCalculator.IsAtLeast(dob, Years) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
}

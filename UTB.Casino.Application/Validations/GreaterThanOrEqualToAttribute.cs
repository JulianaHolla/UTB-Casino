using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.Validations;

// Own validation attribute: value must not be lower than another property (e.g. MaxBet >= MinBet).
// Client side part is in the Web project (Validations/Adapters).
[AttributeUsage(AttributeTargets.Property)]
public class GreaterThanOrEqualToAttribute : ValidationAttribute
{
    public string OtherProperty { get; }

    public GreaterThanOrEqualToAttribute(string otherProperty)
    {
        OtherProperty = otherProperty;
        ErrorMessage = "{0} must be greater than or equal to " + otherProperty + ".";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(OtherProperty);
        if (property == null) return new ValidationResult($"Unknown property {OtherProperty}.");

        var other = property.GetValue(validationContext.ObjectInstance);
        if (value is IComparable current && other != null && current.CompareTo(other) < 0)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        return ValidationResult.Success;
    }
}

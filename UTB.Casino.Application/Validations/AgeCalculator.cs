namespace UTB.Casino.Application.Validations;

public static class AgeCalculator
{
    public static bool IsAtLeast(DateTime dateOfBirth, int years, DateTime? today = null)
    {
        var now = (today ?? DateTime.Today).Date;
        var age = now.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > now.AddYears(-age)) age--;
        return age >= years;
    }
}

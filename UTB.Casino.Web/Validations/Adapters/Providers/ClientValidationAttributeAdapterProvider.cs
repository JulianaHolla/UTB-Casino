using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using UTB.Casino.Application.Validations;

namespace UTB.Casino.Web.Validations.Adapters.Providers;

public class ClientValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _baseProvider;

    public ClientValidationAttributeAdapterProvider(ValidationAttributeAdapterProvider baseProvider) => _baseProvider = baseProvider;

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer) => attribute switch
    {
        MinimumAgeAttribute minimumAge => new MinimumAgeAttributeAdapter(minimumAge, stringLocalizer),
        GreaterThanOrEqualToAttribute greater => new GreaterThanOrEqualToAttributeAdapter(greater, stringLocalizer),
        _ => _baseProvider.GetAttributeAdapter(attribute, stringLocalizer)
    };
}

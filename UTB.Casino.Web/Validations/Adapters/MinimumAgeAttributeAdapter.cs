using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using UTB.Casino.Application.Validations;

namespace UTB.Casino.Web.Validations.Adapters;

public class MinimumAgeAttributeAdapter : AttributeAdapterBase<MinimumAgeAttribute>
{
    public MinimumAgeAttributeAdapter(MinimumAgeAttribute attribute, IStringLocalizer? stringLocalizer)
        : base(attribute, stringLocalizer) { }

    public override void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-minimumage", Attribute.ErrorMessage ?? "");
        MergeAttribute(context.Attributes, "data-val-minimumage-years", Attribute.Years.ToString());
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext) =>
        Attribute.ErrorMessage ?? "";
}

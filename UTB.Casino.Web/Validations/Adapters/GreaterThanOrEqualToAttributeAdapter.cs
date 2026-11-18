using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using UTB.Casino.Application.Validations;

namespace UTB.Casino.Web.Validations.Adapters;

public class GreaterThanOrEqualToAttributeAdapter : AttributeAdapterBase<GreaterThanOrEqualToAttribute>
{
    public GreaterThanOrEqualToAttributeAdapter(GreaterThanOrEqualToAttribute attribute, IStringLocalizer? stringLocalizer)
        : base(attribute, stringLocalizer) { }

    public override void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-greaterthanorequalto", GetErrorMessage(context));
        MergeAttribute(context.Attributes, "data-val-greaterthanorequalto-other", Attribute.OtherProperty);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext) =>
        Attribute.FormatErrorMessage(validationContext.ModelMetadata.GetDisplayName());
}

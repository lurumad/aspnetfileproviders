using FluentValidation;
using RazorEmailTemplates.Features.Shared;

namespace RazorEmailTemplates.Features.Details
{
    public class TemplateDetailsValidator : AbstractValidator<TemplateDetailsModel>
    {
        public TemplateDetailsValidator()
        {
            RuleFor(x => x.Language).Custom((value, action) =>
            {
                if (!LanguageValidator.IsValid(value))
                {
                    action.AddFailure($"{value} is not a valid language");
                }
            });
        }
    }
}

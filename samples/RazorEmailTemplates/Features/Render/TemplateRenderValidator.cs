using FluentValidation;
using RazorEmailTemplates.Features.Shared;

namespace RazorEmailTemplates.Features.Render
{
    public class TemplateRenderValidator : AbstractValidator<TemplateRenderModel>
    {
        public TemplateRenderValidator()
        {
            RuleFor(x => x.Template).NotEmpty().WithMessage("Template name shoud not be empty.");
            RuleFor(x => x.Language).Custom((value, action) =>
            {
                if (!LanguageValidator.IsValid(value))
                {
                    action.AddFailure($"{value} is not a valid language");
                }
            });
            RuleFor(x => x.Model).NotNull().WithMessage("Model should not be null.");
        }
    }
}

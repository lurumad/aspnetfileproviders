using FluentValidation;
using RazorEmailTemplates.Features.Shared;

namespace RazorEmailTemplates.Features.Delete
{
    public class TemplateDeleteValidator : AbstractValidator<TemplateDeleteModel>
    {
        public TemplateDeleteValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Template name shoud not be empty.");
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

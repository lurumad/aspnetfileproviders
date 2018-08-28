using FluentValidation;
using RazorEmailTemplates.Features.Shared;
using System;
using System.Text;

namespace RazorEmailTemplates.Features.PaginatedList
{
    public class PaginatedListValidator : AbstractValidator<PaginatedListModel>
    {
        public PaginatedListValidator()
        {
            RuleFor(x => x.Language).Custom((value, action) =>
            {
                if (!LanguageValidator.IsValid(value))
                {
                    action.AddFailure($"{value} is not a valid language");
                }
            });
            When(x => !String.IsNullOrWhiteSpace(x.ContinuationToken), () =>
                RuleFor(x => x.ContinuationToken).Custom((value, action) =>
                {
                    Span<byte> bytes = Encoding.UTF8.GetBytes(value);
                    if (!Convert.TryFromBase64String(value, bytes, out int bytesWritten))
                    {
                        action.AddFailure($"{value} is not a valid Base64 value");
                    }
                })
            );
        }
    }
}

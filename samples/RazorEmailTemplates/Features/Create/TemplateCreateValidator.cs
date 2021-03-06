﻿using FluentValidation;
using RazorEmailTemplates.Features.Shared;
using System;
using System.IO;

namespace RazorEmailTemplates.Features.Create
{
    public class TemplateCreateValidator : AbstractValidator<TemplateCreateModel>
    {
        public TemplateCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Template name shoud not be empty.");
            RuleFor(x => x.Template).Custom((value, action) =>
            {
                if (value == null || !Path.GetExtension(value.FileName).Equals(".cshtml", StringComparison.InvariantCultureIgnoreCase))
                {
                    action.AddFailure("Only cshtml files are allowed.");
                }
            });
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

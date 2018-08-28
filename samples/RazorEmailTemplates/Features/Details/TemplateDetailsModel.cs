using Microsoft.AspNetCore.Mvc;

namespace RazorEmailTemplates.Features.Details
{
    public class TemplateDetailsModel
    {
        [FromRoute]
        public string Name { get; set; }
        [FromRoute]
        public string Language { get; set; }
    }
}

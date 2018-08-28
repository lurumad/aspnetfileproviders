using Microsoft.AspNetCore.Http;

namespace RazorEmailTemplates.Features.Create
{
    public class TemplateCreateModel
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public IFormFile Template { get; set; }
    }
}

using Microsoft.AspNetCore.Http;

namespace RazorEmailTemplates.Features.Update
{
    public class TemplateUpdateModel
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public IFormFile Template { get; set; }
    }
}

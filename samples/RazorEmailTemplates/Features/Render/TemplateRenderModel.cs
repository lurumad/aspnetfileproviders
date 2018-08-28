namespace RazorEmailTemplates.Features.Render
{
    public class TemplateRenderModel
    {
        public string Template { get; set; }
        public string Language { get; set; }
        public object Model { get; set; }
        public bool Stringify { get; set; }
    }
}

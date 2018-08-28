namespace RazorEmailTemplates.Features.Details
{
    public class TemplateDetails
    {
        public TemplateDetails(string name, string language, string content)
        {
            Name = name;
            Language = language;
            Content = content;
        }

        public string Name { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}

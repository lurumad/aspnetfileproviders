namespace RazorEmailTemplates.Features.PaginatedList
{
    public class TemplatePagedResult
    {
        public TemplatePagedResult(string name, string language)
        {
            Name = name;
            Language = language;
        }

        public string Name { get; set; }
        public string Language { get; set; }
    }
}

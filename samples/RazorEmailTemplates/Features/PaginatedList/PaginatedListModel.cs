using Microsoft.AspNetCore.Mvc;

namespace RazorEmailTemplates.Features.PaginatedList
{
    public class PaginatedListModel
    {
        [FromRoute]
        public string Language { get; set; }
        [FromQuery]
        public string ContinuationToken { get; set; }
    }
}

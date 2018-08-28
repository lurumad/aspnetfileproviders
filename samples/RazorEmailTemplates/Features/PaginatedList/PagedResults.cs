using System.Collections.Generic;

namespace RazorEmailTemplates.Features.PaginatedList
{
    public class PagedResults   
    {
        public PagedResults(
            IEnumerable<TemplatePagedResult> results,
            string continuationToken,
            bool hasMoreResults)
        {
            Results = results;
            ContinuationToken = continuationToken;
            HasMoreResults = hasMoreResults;
        }

        public IEnumerable<TemplatePagedResult> Results { get; }
        public string ContinuationToken { get;  }
        public bool HasMoreResults { get; }
    }
}

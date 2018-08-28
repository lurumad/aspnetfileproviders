using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Features.Delete;
using RazorEmailTemplates.Features.Details;
using RazorEmailTemplates.Features.PaginatedList;
using RazorEmailTemplates.Features.Update;
using System.Threading.Tasks;

namespace RazorEmailTemplates.Repositories
{
    public interface ITemplatesRepository
    {
        Task Add(TemplateCreateModel model);
        Task Update(TemplateUpdateModel model);
        Task Delete(TemplateDeleteModel model);
        Task<PagedResults> GetPagedResults(string language, string continuationToken);
        Task<TemplateDetails> Details(string language, string name);
    }
}

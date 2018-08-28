using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Features.Delete;
using RazorEmailTemplates.Features.Details;
using RazorEmailTemplates.Features.PaginatedList;
using RazorEmailTemplates.Features.Render;
using RazorEmailTemplates.Features.Update;
using System.Threading.Tasks;

namespace RazorEmailTemplates.Services
{
    public interface ITemplatesService
    {
        Task Add(TemplateCreateModel model);
        Task<string> Render(TemplateRenderModel model);
        Task Update(TemplateUpdateModel model);
        Task Delete(TemplateDeleteModel model);
        Task<PagedResults> GetPagedResults(string language, string continuationToken);
        Task<TemplateDetails> Details(string language, string name);
    }
}
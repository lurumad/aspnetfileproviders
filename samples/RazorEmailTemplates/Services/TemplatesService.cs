using Newtonsoft.Json;
using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Features.Delete;
using RazorEmailTemplates.Features.Details;
using RazorEmailTemplates.Features.PaginatedList;
using RazorEmailTemplates.Features.Render;
using RazorEmailTemplates.Features.Update;
using RazorEmailTemplates.Infrastructure.Engine;
using RazorEmailTemplates.Repositories;
using System.Threading.Tasks;

namespace RazorEmailTemplates.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly IViewToStringRenderer viewToStringRenderer;
        private readonly ITemplatesRepository templatesRepository;

        public TemplatesService(
            IViewToStringRenderer viewToStringRenderer,
            ITemplatesRepository templatesRepository)
        {
            this.viewToStringRenderer = viewToStringRenderer ?? throw new System.ArgumentNullException(nameof(viewToStringRenderer));
            this.templatesRepository = templatesRepository ?? throw new System.ArgumentNullException(nameof(templatesRepository));
        }

        public Task Add(TemplateCreateModel model)
        {
            return templatesRepository.Add(model);
        }

        public Task Update(TemplateUpdateModel model)
        {
            return templatesRepository.Update(model);
        }

        public async Task<string> Render(TemplateRenderModel model)
        {
            var html = await viewToStringRenderer.RenderViewToStringAsync(
                $"{model.Language.ToLowerInvariant()}/{model.Template}/{model.Template}.cshtml", model.Model);

            return model.Stringify ? JsonConvert.SerializeObject(html) : html;
        }

        public Task Delete(TemplateDeleteModel model)
        {
            return templatesRepository.Delete(model);
        }

        public Task<PagedResults> GetPagedResults(string language, string continuationToken)
        {
            return templatesRepository.GetPagedResults(language, continuationToken);
        }


        public Task<TemplateDetails> Details(string language, string name)
        {
            return templatesRepository.Details(language, name);
        }
    }
}

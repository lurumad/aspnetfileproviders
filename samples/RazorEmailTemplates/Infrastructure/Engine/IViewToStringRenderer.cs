using System.Threading.Tasks;

namespace RazorEmailTemplates.Infrastructure.Engine
{
    public interface IViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}

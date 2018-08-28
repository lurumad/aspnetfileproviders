using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Features.Delete;
using RazorEmailTemplates.Features.Details;
using RazorEmailTemplates.Features.PaginatedList;
using RazorEmailTemplates.Features.Render;
using RazorEmailTemplates.Features.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorEmailTemplates.Services;
using System.Threading.Tasks;

namespace RazorEmailTemplates.Features
{
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplatesService templatesService;

        public TemplatesController(ITemplatesService templatesService)
        {
            this.templatesService = templatesService ?? throw new System.ArgumentNullException(nameof(templatesService));
        }

        [HttpGet]
        [Route("templates/{language}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaginated(PaginatedListModel query)
        {
            var results = await templatesService.GetPagedResults(query.Language, query.ContinuationToken);
            return Ok(results);
        }

        [HttpGet]
        [Route("templates/{language}/{name}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(TemplateDetailsModel query)
        {
            var result = await templatesService.Details(query.Language, query.Name);
            return Ok(result);
        }

        [HttpPost]
        [Route("templates")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(TemplateCreateModel model)
        {
            await templatesService.Add(model);
            return Created("",null);
        }

        [HttpPut]
        [Route("templates")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(TemplateUpdateModel model)
        {
            await templatesService.Update(model);
            return Ok();
        }

        [HttpDelete]
        [Route("templates")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] TemplateDeleteModel model)
        {
            await templatesService.Delete(model);
            return NoContent();
        }

        [HttpPost]
        [Route("templates/render")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ContentResult> Render([FromBody] TemplateRenderModel model)
        {
            var content = await templatesService.Render(model);
            return Content(content);
        }
    }
}

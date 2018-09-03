using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Core.Ports.Shared;
using ProductCatalog.Domain.Products.InputModels;
using ProductCatalog.Domain.Products.Ports.Driving;
using ProductCatalog.Domain.Products.Statuses;

namespace ProductCatalog.Api.Controllers
{
	[ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
	[FormatFilter]
    public class ProductsController: ControllerBase
    {        
		private IProductCommandsHandler productCommandsHandler;
		private IProductQueriesHandler productQueriesHandler;
		private ILogger logger;

		public ProductsController(IProductCommandsHandler productCommandsHandler, IProductQueriesHandler productQueriesHandler, ILogger logger)
		{
			this.productCommandsHandler = productCommandsHandler ?? throw new ArgumentNullException(nameof(productCommandsHandler));
			this.productQueriesHandler = productQueriesHandler ?? throw new ArgumentNullException(nameof(productQueriesHandler));
			this.logger = logger;
		}

		
 		//[Route("[controller]/[action]/{id}.{format?}")]
        
        [HttpGet]
        public async Task<IActionResult> GetItemsAsync(string search)
        {
			try
			{
				var products = string.IsNullOrWhiteSpace(search)
					? await productQueriesHandler.GetAllAsync()
					: await productQueriesHandler.SearchAsync(search);

				return Ok(products);
			}
			catch (Exception exp)
			{
				logger?.LogException(exp);
				return StatusCode(500);
			}
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
			try
			{
				var product = await productQueriesHandler.GetAsync(id);

				return product != null
					? (IActionResult) Ok(product)
					: NotFound();
			}
			catch (Exception exp)
			{
				logger?.LogException(exp);
				return StatusCode(500);
			}
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ProductInputModel value)
        {
			try
			{
				if (!ModelState.IsValid)
					return BadRequest();

				return await productCommandsHandler.CreateAsync(value) 
					? (IActionResult)StatusCode(201) 
					: BadRequest(true);
			}
			catch (Exception exp)
			{
				logger?.LogException(exp);
				return StatusCode(500);
			}
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] ProductInputModel value)
        {
			try
			{
				if (!ModelState.IsValid)
					return BadRequest();

				var result = await productCommandsHandler.UpdateAsync(id, value);
				switch(result)
				{
					case EUpdateProductCommandStatus.Success:
						return Ok();
					case EUpdateProductCommandStatus.ProductNotExists:
						return NotFound();
					case EUpdateProductCommandStatus.FailsBecauseDuplicatedCode:
						return BadRequest((int)result);
					default:
						throw new NotSupportedException($"Result: '{result.ToString()}'({(int)result})");
				}
			}
			catch (Exception exp)
			{
				logger?.LogException(exp);
				return StatusCode(500);
			}
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
			try
			{
				return await productCommandsHandler.DeleteAsync(id) 
					? (IActionResult)NoContent() 
					: NotFound();
			}
			catch (Exception exp)
			{
				logger?.LogException(exp);
				return StatusCode(500);
			}
		}
	}
}
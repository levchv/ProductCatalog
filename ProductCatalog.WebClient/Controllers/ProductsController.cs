using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductCatalog.WebClient.Configurations;
using ProductCatalog.WebClient.Exceptions;
using ProductCatalog.WebClient.Models;

namespace ProductCatalog.WebClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebApiClientConfiguration _webApiClientConfiguration;

        public ProductsController(IOptions<WebApiClientConfiguration> options)
        {
			_webApiClientConfiguration = options.Value;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new ProductsViewModel
                {
                    ExcelLink = Path.Combine(_webApiClientConfiguration.Url, "products?format=csv"),
                    Products = await GetDataAsync<List<ProductViewModel>>("products")
                };
                return View(model);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        public async Task<IActionResult> Search(string search)
        {
            try
            {
                var model = new ProductsViewModel
                {
                    Search = search,
                    ExcelLink = Path.Combine(_webApiClientConfiguration.Url, $"products?search={search}&format=csv"),
                    Products = await GetDataAsync<List<ProductViewModel>>($"products?search={search}")
                };
                return View(nameof(Index), model);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var model = await GetDataAsync<ProductViewModel>($"products/{id}");
                return model == null 
                    ? (IActionResult)NotFound() 
                    : View(model);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Price,Photo")] ProductInputModel product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(product);

                await RunCommandAsync(async (client) => await client.PostAsync("products", product, new JsonMediaTypeFormatter()));

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestExceptionWithStatusCode exp) when (bool.TryParse(exp.Value?.ToString(), out var value) && value)
            {
                 ModelState.AddModelError(nameof(ProductViewModel.Code), "Already exists");
                 return View(product);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var model = await GetDataAsync<ProductInputModel>($"products/{id}");
                return model == null
                    ? (IActionResult)NotFound()
                    : View(model);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Price,Photo")] ProductInputModel product)
        {
            if (id != product.Id)
                return NotFound();

            try
            {
                if (!ModelState.IsValid)
                    return View(product);

                await RunCommandAsync(async (client) => await client.PutAsync($"products/{id}", product, new JsonMediaTypeFormatter()));

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestExceptionWithStatusCode exp) when (int.TryParse(exp.Value?.ToString(), out var value) && value == 3)
            {
                ModelState.AddModelError(nameof(ProductViewModel.Code), "Already exists");
                return View(product);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var model = await GetDataAsync<ProductViewModel>($"products/{id}");
                return model == null
                    ? (IActionResult)NotFound()
                    : View(model);
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await RunCommandAsync(async (client) => await client.DeleteAsync($"products/{id}"));

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exp)
            {
                return HandleException(exp);
            }
        }

        #region Additional functions

        private async Task RunCommandAsync(Func<HttpClient, Task<HttpResponseMessage>> command)
        {
            using (var client = GetWebApiClient())
            {
                var result = await command(client);
                if (result.IsSuccessStatusCode)
                    return;

                throw new HttpRequestExceptionWithStatusCode(result.StatusCode, await result.Content.ReadAsAsync<object>());
            }
        }

        private async Task<T> GetDataAsync<T>(string path)
        {
            using (var client = GetWebApiClient())
            {
                var result = await client.GetAsync(path);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data);
                }
                throw new HttpRequestExceptionWithStatusCode(result.StatusCode);
            }
        }

        private HttpClient GetWebApiClient()
        {
            var client = new HttpClient();
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_webApiClientConfiguration.Url);
            }
            catch
            {
                client.Dispose();
                throw;
            }
            return client;
        }

        private IActionResult HandleException(Exception exp)
        {
            if (exp is HttpRequestExceptionWithStatusCode expWithStatusCode
                    && expWithStatusCode.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            return View("Error", new ErrorViewModel { RequestId = Request.Path.Value });
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalog.Api.Configurations;
using ProductCatalog.Domain.Products.Adapters.Commands;
using ProductCatalog.Domain.Products.Adapters.Queries;
using ProductCatalog.Domain.Products.Ports.Driving;
using ProductCatalog.Infrastructure.Products.Adapters.Factories;

namespace ProductCatalog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			var configuration = Configuration.GetSection("Database").Get<DatabaseConfiguration>();
			var productRepositoryFactory = new ProductRepositoryFactory(configuration.ConnectionString);			
			var productReadOnlyRepositoryFactory = new ProductReadOnlyRepositoryFactory(configuration.ConnectionString);
			var loggerFactory = new ProductCatalog.Infrastructure.Shared.Adapters.Loggers.LoggerFactory();

			services.AddScoped<IProductCommandsHandler>((s) => new ProductCommandsHandler(productRepositoryFactory));
			services.AddTransient<IProductQueriesHandler>((s) => new ProductQueriesHandler(productReadOnlyRepositoryFactory.Get()));
			services.AddSingleton<ProductCatalog.Domain.Core.Ports.Shared.ILogger>((s) => loggerFactory.Get());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

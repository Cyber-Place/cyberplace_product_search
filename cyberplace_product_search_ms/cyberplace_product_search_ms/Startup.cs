using cyberplace_product_search_ms.Models;
using cyberplace_product_search_ms.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms
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
            //Dependency injection productsearch settings 
            services.Configure<ProductSearchSettings>
                (Configuration.GetSection(nameof(ProductSearchSettings)));
            services.AddSingleton<IProductSearchSettings>
                (d => d.GetRequiredService<IOptions<ProductSearchSettings>>().Value);
            //Dependency injection productsearch settings
            services.Configure<RabbitMQSettings>
                (Configuration.GetSection(nameof(RabbitMQSettings)));
            services.AddSingleton<IRabbitMQSettings>
                (d => d.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

            services.AddHostedService<RabbitReceiverService>();


            services.AddSingleton<SearchItemService>();
            services.AddSingleton<SearchHistoryService>();
            services.AddControllers();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "cyberplace_product_search_ms", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "cyberplace_product_search_ms v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

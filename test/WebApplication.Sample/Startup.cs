using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Extensions;
using QbSync.WebConnector.Impl;
using WebApplication.Sample.Application;
using WebApplication.Sample.Application.Steps;
using WebApplication.Sample.Db;

namespace WebApplication.Sample
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
            services
                .AddWebConnector(options =>
                {
                    options
                        .AddAuthenticator<Authenticator>()
                        .WithWebConnectorHandler<WebConnectorHandler>()
                        .WithStep<CustomerQuery.Request, CustomerQuery.Response>()
                        .WithStep<InvoiceQuery.Request, InvoiceQuery.Response>()
                        .WithStep<CustomerAdd.Request, CustomerAdd.Response>();
                });

            services
                .AddMvc();

            services
                .AddScoped<ApplicationDbContext>();

            var context = new ApplicationDbContext();
            context.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseWebConnector(options =>
                {
                    options.SoapPath = "/QBConnectorAsync.asmx";
                });

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

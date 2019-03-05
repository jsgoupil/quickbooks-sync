using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QbSync.WebConnector.Extensions;
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
            // Register the web connector like this.
            // The steps are automatically registered as Scoped.
            // You can change the lifetime if required.
            services
                .AddWebConnector(options =>
                {
                    options
                        .AddAuthenticator<Authenticator>()
                        .WithWebConnectorHandler<WebConnectorHandler>()
                        .WithStep<CustomerGroupAddQuery.Request, CustomerGroupAddQuery.Response>()
                        .WithStep<CustomerQuery.Request, CustomerQuery.Response>()
                        .WithStep<InvoiceQuery.Request, InvoiceQuery.Response>()
                        .WithStep<CustomerAdd.Request, CustomerAdd.Response>()

                        // You need to do some work in this InvoiceAdd class before you can activate it.
                        ////.WithStep<InvoiceAdd.Request, InvoiceAdd.Response>()
                    ;
                });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddScoped<ApplicationDbContext>();

            var context = new ApplicationDbContext();
            context.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Indicates the path for your service to be reached.
            app
                .UseWebConnector(options =>
                {
                    options.SoapPath = "/QBConnectorAsync.asmx";
                });

            if (env.IsDevelopment())
            {
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

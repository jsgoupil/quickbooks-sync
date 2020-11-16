using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddControllersWithViews();

            services
                .AddScoped<ApplicationDbContext>();

            var context = new ApplicationDbContext();
            context.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

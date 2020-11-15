using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransferVendorData.Web.Services;
using TransferVendorData.Web.Services.Base;
using Microsoft.AspNet.OData.Extensions;
using TransferVendorData.Web.Models;
using TransferVendorData.Web.Repository.Base;
using TransferVendorData.Web.Repository;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace TransferVendorData.Web
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
            var connection = Configuration.GetConnectionString("ERPDatabase");
            services.AddDbContextPool<ERPContext>(options => options.UseSqlServer(connection));

            // services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllers(mvcOptions =>
            mvcOptions.EnableEndpointRouting = false);

            services.AddOData();

            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorBankAccountRepository, VendorBankAccountRepository>();
            services.AddHttpClient<IDataRetrievalService, DynamicsDataRetrievalService>(client =>
            {
                client.BaseAddress = new Uri(string.Format("{0}/{1}", Configuration["Endpoints:redirectUrl"], Configuration["Endpoints:odata"]));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Filter();
                routeBuilder.EnableDependencyInjection();
            });
        }
    }
}

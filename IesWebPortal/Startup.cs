using IQToolkit.Data.Common;
using IQToolkit.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SageClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using IesWebPortal.Services.Interfaces;
using IesWebPortal.Services;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using IesWebPortal.Models;

namespace IesWebPortal
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
            services.AddControllersWithViews();
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);

            services.AddOData();
            services.AddTransient<SageDataContext>(x => CreateSageDataContext(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IMemoManager>(x => CreateMemoManager(x.GetService<IIesWebPortalSettings>()));
            services.AddTransient<IDataService>(x => new DataService(x.GetService<SageDataContext>(), x.GetService<IMemoManager>(), null));
        }

        private static IMemoManager CreateMemoManager(IIesWebPortalSettings iesWebPortalSettings)
        {
            return new MemoManager(iesWebPortalSettings.RiskCapture, iesWebPortalSettings.EiniecsCapture, iesWebPortalSettings.UnCapture, iesWebPortalSettings.PictureCapture);
        }

        private static SageDataContext CreateSageDataContext(string connStr)
        {
            var sqlconn = new SqlConnection(connStr);
            sqlconn.Open();
            var sqldbqueryprovider = new SqlQueryProvider(
                        sqlconn,
                        new SageClassLibrary.CustomMapping(TSqlLanguage.Default as QueryLanguage),
                        QueryPolicy.Default);
            //sqldbqueryprovider.Log = Console.Out;
            return new SageClassLibrary.SageDataContext(sqldbqueryprovider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<FlashPoint>("FlashPoints");
            odataBuilder.EntitySet<Inventory>("Inventories");
            return odataBuilder.GetEdmModel();
        }

    }
}

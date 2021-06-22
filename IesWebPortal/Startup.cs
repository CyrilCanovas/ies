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
using IesWebPortal.Model;
using System.IO;
using IesWebPortal.Classes;

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
            services.AddTransient<IDataService>(x => CreateDataService(x.GetService<SageDataContext>(), x.GetService<IMemoManager>(), x.GetService<IIesWebPortalSettings>(),x.GetService<IWebHostEnvironment>()));
            
            services.AddSingleton<IMLLabelConfigs>(x => GetLabelConfigs(x.GetService<IIesWebPortalSettings>()));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static IMemoManager CreateMemoManager(IIesWebPortalSettings iesWebPortalSettings)
        {
            return new MemoManager(iesWebPortalSettings.RiskCapture, iesWebPortalSettings.EiniecsCapture, iesWebPortalSettings.UnCapture, iesWebPortalSettings.PictureCapture);
        }

        private static IDataService CreateDataService(SageDataContext sageDataContext,IMemoManager memoManager,IIesWebPortalSettings iesWebPortalSettings, IWebHostEnvironment environment)
        {
            byte[] emptyPictureContent = null;
            if (!string.IsNullOrEmpty(iesWebPortalSettings.EmptyPicture))
            {
                try
                {
                    var pathRoot = Path.GetPathRoot(iesWebPortalSettings.EmptyPicture);
                    string fileName = null;
                    if (string.IsNullOrWhiteSpace(pathRoot) || pathRoot =="/" || pathRoot == @"\")
                    {
                        fileName = Path.Combine(environment.WebRootPath, IesWebPortalConstants.PICTURE_PATH, iesWebPortalSettings.EmptyPicture);
                    }
                    else
                    {
                        fileName = iesWebPortalSettings.EmptyPicture;
                    }
                    emptyPictureContent = File.ReadAllBytes(fileName);
                }
                catch(Exception ex)
                {

                }

                
            }
            return new DataService(sageDataContext, memoManager, emptyPictureContent);
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

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<FlashPoint>("FlashPoints");
            odataBuilder.EntitySet<Inventory>("Inventories");
            return odataBuilder.GetEdmModel();
        }

        private static IMLLabelConfigs GetLabelConfigs(IIesWebPortalSettings iesWebPortalSettings)
        {
            var configs =
                new MLLabelConfig[] {
                    new MLLabelConfig()
                    {
                        ReportName = "BigLabel.rdlc",
                        Description = "Etiquettes grand format",
                        Settings = iesWebPortalSettings.LabelSettings.ContainsKey("BigLabelSettings") ? iesWebPortalSettings.LabelSettings["BigLabelSettings"] : string.Empty
                    },
                    new MLLabelConfig()
                    {
                        ReportName = "SmallLabel.rdlc",
                        Description = "Etiquettes petit format",
                        Settings = iesWebPortalSettings.LabelSettings.ContainsKey("SmallLabelSettings") ? iesWebPortalSettings.LabelSettings["SmallLabelSettings"] : string.Empty
                    },
                    new MLLabelConfig()
                    {
                        ReportName = "SampleLabel.rdlc",
                        Description = "Etiquettes echantillons",
                        Settings = iesWebPortalSettings.LabelSettings.ContainsKey("SampleLabelSettings") ? iesWebPortalSettings.LabelSettings["SampleLabelSettings"] : string.Empty
                        //Settings=global::IesWebPortal.Properties.Settings.Default.    
                    }
                    ,
                    //new LabelConfig(){ReportName="Report12x3.rdlc",
                    //    Description="Etiquettes echantillons 12x3 (ancien)",
                    //    Settings=null,
                    //    ColCount=3,
                    //    RowCount=12
                    //}
                    //,
                    new MLLabelConfig()
                    {
                        ReportName = "Report16x4.rdlc",
                        Description = "Etiquettes echantillons 16x4",
                        Settings = null,
                        ColCount = 4,
                        RowCount = 16
                    },
                    new MLLabelConfig()
                    {
                        ReportName = "DeliveryLabel.rdlc",
                        Description = "Etiquette livraison client",
                        Settings = iesWebPortalSettings.LabelSettings.ContainsKey("DeliveryLabelSettings") ? iesWebPortalSettings.LabelSettings["DeliveryLabelSettings"] : string.Empty
                    },
                    new MLLabelConfig()
                    {
                        ReportName = "BarrelProduct.rdlc",
                        Description = "Etiquette produit fut",
                        Settings = iesWebPortalSettings.LabelSettings.ContainsKey("BarrelPrroductLabelSettings") ? iesWebPortalSettings.LabelSettings["BarrelPrroductLabelSettings"] : string.Empty
                    }


                    
                };
            return new MLLabelConfigs(configs);
        }

    }
}

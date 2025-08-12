using System;
using System.Collections.Generic;
using System.Windows.Forms;
// for tracing
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using DiversityCollection.Forms;
using DiversityWorkbench.Forms;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using Microsoft.Extensions.DependencyInjection;
using DiversityWorkbench;
using DWBServices;
using DWBServices.WebServices.GeoServices.Geonames;
using DWBServices.WebServices.GeoServices.IHOWorldSeas;
using DWBServices.WebServices.GeoServices.ISOCountries;
using DWBServices.WebServices.TaxonomicServices.GbifSpecies;
using DWBServices.WebServices.TaxonomicServices.GfbioTerminology;
using DWBServices.WebServices.TaxonomicServices.IndexFungorum;
using DWBServices.WebServices.TaxonomicServices.Mycobank;
using DWBServices.WebServices.TaxonomicServices.PESI;
using DWBServices.WebServices.TaxonomicServices.WoRMS;
using Microsoft.Extensions.Configuration;
using DWBServices.WebServices.GeoServices.GFBioTermGeonames;

namespace DiversityCollection
{
    static class Program
    {
        // for tracing
        private static TraceSource traceSource = new TraceSource("TraceSourceApp");
        // ServiceProvider for e.g. WebServices
        private static IServiceProvider DwbServiceProvider { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));
            
            
            if (args.Length == 0)
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                DwbServiceProvider = serviceCollection.BuildServiceProvider();
                DwbServiceProviderAccessor.Instance = DwbServiceProvider;
                Application.Run(DwbServiceProvider.GetRequiredService<FormCollectionSpecimen>());
            }
            else if (args.Length == 1)
            {
                int Port = System.Int32.Parse(args[0]);
                //DiversityCollection.ChannelRegistration.RegisterTcpChannelServer(Port);
                //DiversityCollection.ChannelRegistration.RegisterTcpServices();
                Application.Run();
            }
            else
            {
                Application.Run(new Forms.FormCollectionSpecimen(args));
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var dwbconfiguration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("apisettings.json", false, true)
                //.AddUserSecrets(typeof(Program).Assembly)
                .Build();
            // Register IConfiguration in the DI container
            services.AddSingleton<IConfiguration>(dwbconfiguration);
            // register services here
            services.AddTransient<FormCollectionSpecimen>();
            services.AddTransient<FormRemoteQuery>();
            services.AddHttpClient<CoLWebservice>();
            services.AddHttpClient<IndexFungorumWebservice>();
            services.AddHttpClient<PESIWebservice>();
            services.AddHttpClient<MycobankWebservice>();
            services.AddHttpClient<GeonamesWebservice>();
            services.AddHttpClient<GFBioTermGeonamesWebservice>();
            services.AddHttpClient<IsoCountriesWebservice>();
            services.AddHttpClient<IHOWorldSeasWebservice>();
            services.AddHttpClient<GbifSpeciesWebservice>();
            services.AddHttpClient<WoRMSWebservice>();
            services.AddHttpClient<GfbioTerminologyWebservice>();
        }
    }
}
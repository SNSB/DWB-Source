using DiversitySamplingPlots.Forms;
using DiversityWorkbench.Forms;
using DWBServices;
using DWBServices.WebServices.GeoServices.Geonames;
using DWBServices.WebServices.GeoServices.GFBioTermGeonames;
using DWBServices.WebServices.GeoServices.IHOWorldSeas;
using DWBServices.WebServices.GeoServices.ISOCountries;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using DWBServices.WebServices.TaxonomicServices.GbifSpecies;
using DWBServices.WebServices.TaxonomicServices.GfbioTerminology;
using DWBServices.WebServices.TaxonomicServices.IndexFungorum;
using DWBServices.WebServices.TaxonomicServices.Mycobank;
using DWBServices.WebServices.TaxonomicServices.PESI;
using DWBServices.WebServices.TaxonomicServices.WoRMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DiversitySamplingPlots
{
    static class Program
    {
        // ServiceProvider for e.g. WebServices
        private static IServiceProvider DwbServiceProvider { get; set; }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));


            if (args.Length == 0)
            { 
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                using (var serviceProvider = serviceCollection.BuildServiceProvider())
                {
                    DwbServiceProvider = serviceProvider;
                    DwbServiceProviderAccessor.Instance = DwbServiceProvider;
                    Application.Run(DwbServiceProvider.GetRequiredService<FormDiversitySamplingPlots>());
                }
            }
            else if (args.Length == 1)
            {
                int Port = System.Int32.Parse(args[0]);
                Application.Run();
            }
            else
            {
                Application.Run(new Forms.FormDiversitySamplingPlots(args));
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
            services.AddTransient<FormDiversitySamplingPlots>();
            services.AddTransient<FormRemoteQuery>();
            services.AddHttpClient<GeonamesWebservice>();
            services.AddHttpClient<GFBioTermGeonamesWebservice>();
            services.AddHttpClient<IsoCountriesWebservice>();
            services.AddHttpClient<IHOWorldSeasWebservice>();
        }
    }
}
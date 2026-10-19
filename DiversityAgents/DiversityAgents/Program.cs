using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DiversityWorkbench.Forms;
using DWBServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiversityAgents
{
    static class Program
    {
        private static IServiceProvider DwbServiceProvider { get; set; }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "<Ausstehend>")]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                DwbServiceProvider = serviceCollection.BuildServiceProvider();
                DwbServiceProviderAccessor.Instance = DwbServiceProvider;
                Application.Run(DwbServiceProvider.GetRequiredService<FormDiversityAgents>());
                // Application.Run(new FormDiversityAgents());
            }
            else if (args.Length == 1)
            {
                int Port = System.Int32.Parse(args[0]);
                Application.Run();
            }
            else
            {
                Application.Run(new FormDiversityAgents(args));
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
            services.AddTransient<FormDiversityAgents>();

        }
    }
}

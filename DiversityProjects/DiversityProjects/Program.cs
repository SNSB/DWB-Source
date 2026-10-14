using DiversityWorkbench.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DWBServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiversityProjects
{
    static class Program
    {
        private static IServiceProvider DwbServiceProvider { get; set; }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
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
                Application.Run(DwbServiceProvider.GetRequiredService<FormDiversityProjects>());
                // Application.Run(new FormDiversityProjects());
            }
            else if (args.Length == 1)
            {
                int Port = System.Int32.Parse(args[0]);
                Application.Run();
            }
            else
            {
                Application.Run(new FormDiversityProjects(args));
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
            services.AddTransient<FormDiversityProjects>();

        }
    }
}

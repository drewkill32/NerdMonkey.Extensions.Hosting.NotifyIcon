using System;
using System.Windows.Forms;
using NerdMonkey.Extensions.Hosting.NotifyIcon;

namespace NerdMonkey.App
{
    internal static class Program
    {
        public const string APPLICATION_NAME = "HowlerMonkley";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var clientHost = Demo.Program.CreateHostBuilder(args).Build();
            var apiHost = Api.Program.CreateHostBuilder(args).Build();
            Application.Run(new HostedApplicationContext(new []{ clientHost, apiHost }));
        }

    }
}
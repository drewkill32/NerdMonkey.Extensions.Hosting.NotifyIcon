using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
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

            var clientHost = Demo.Program.CreateHostBuilder(args)
                .UseNotifyIcon(configure =>
                {
                    configure.OpenOnStartup = true;
                    configure.Icon = new Icon(@"wwwroot\favicon.ico");
                    configure.Image = configure.Icon.ToBitmap();
                    configure.Url = "http://localhost:5000";
                }).
                Build();

            var apiHost = Api.Program.CreateHostBuilder(args)
                .UseNotifyIcon().Build();

            Application.Run(new HostedApplicationContext(new []{clientHost,apiHost}));
            
        }

    }
}
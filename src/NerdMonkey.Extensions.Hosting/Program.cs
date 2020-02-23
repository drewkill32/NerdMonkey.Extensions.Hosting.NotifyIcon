using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Windows.Forms;

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
            var host = Demo.Program.CreateHostBuilder(args).UseNotifyIcon();
            Application.Run(new TrayApplicationContext(host.Build()));
        }
    }

    public class TrayApplicationContext : ApplicationContext
    {
        private readonly IHost _host;

        private readonly CancellationTokenRegistration _applicationStoppingRegistration;

        public TrayApplicationContext(IHost host)
        {
            _host = host;
            var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            _applicationStoppingRegistration = applicationLifetime.ApplicationStopping.Register(Application.Exit);

            new Thread(Run).Start(); // Otherwise this would block.
        }

        private void Run()
        {
            try
            {
                _host.Run(); // this is blocking
            }
            catch (OperationCanceledException)
            {
                //Ignore
            }

        }

        protected override void Dispose(bool disposing)
        {
            _applicationStoppingRegistration.Dispose();
            base.Dispose(disposing);
        }
    }
}
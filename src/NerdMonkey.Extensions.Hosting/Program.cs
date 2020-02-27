using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
using NerdMonkey.Extensions.Hosting.NotifyIcon;

namespace NerdMonkey.App
{
    internal static class Program
    {

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
                    configure.BuildMenu(m =>
                    {
                        var testMenu = new ToolStripMenuItem("test");
                        testMenu.Click += (sender, eventArgs) => { MessageBox.Show("Test"); };
                        // default menu items are already created. Use Insert to appear at the top of the menu.
                        // m.Items.Clear() can be used to clear the menu if you want to replace the menu entirely.
                        m.Items.Insert(0,testMenu);
                    });
                }).
                Build();

            var apiHost = Api.Program.CreateHostBuilder(args)
                .UseNotifyIcon().Build();
           
            Application.Run(new HostedApplicationContext(new []{clientHost,apiHost}));
            
        }

    }
}
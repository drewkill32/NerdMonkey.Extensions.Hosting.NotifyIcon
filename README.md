# NerdMonkey.Extensions.Hosting.NotifyIcon

![.NET Core](https://github.com/drewkill32/NerdMonkey.Extensions.Hosting.NotifyIcon/workflows/.NET%20Core/badge.svg?branch=master)

![Nuget](https://img.shields.io/nuget/v/NerdMonkey.Extensions.Hosting.NotifyIcon?style=plastic)


NerdMonkey.Extensions.Hosting.NotifyIcon creates a system tray icon and allows the ability to host muliple dotnet web apps inside of a single exe. To use NotifyIcon create a dotnet core Windows Forms app and replace the contents of the `Main` method with the example below.
See Demo App in repo for an example

```csharp
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

```

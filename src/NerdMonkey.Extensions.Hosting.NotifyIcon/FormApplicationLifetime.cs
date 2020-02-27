using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class FormApplicationLifetime:IHostLifetime,IDisposable
    {
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

        public FormApplicationLifetime(IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            HostApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
        }
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _applicationStartedRegistration = HostApplicationLifetime.ApplicationStarted.Register(state =>
                {
                    ((FormApplicationLifetime)state).OnApplicationStarted();
                },
                this);
            _applicationStoppingRegistration = HostApplicationLifetime.ApplicationStopping.Register(state =>
                {
                    ((FormApplicationLifetime)state).OnApplicationStopping();
                },
                this);
            Application.ApplicationExit += Application_ApplicationExit;
            return Task.CompletedTask;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            HostApplicationLifetime.StopApplication();
            if (!_shutdownBlock.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
            }
            _shutdownBlock.WaitOne();
            // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
            // Suppress that since we shut down gracefully. https://github.com/dotnet/aspnetcore/issues/6526
            System.Environment.ExitCode = 0;
        }


        private void OnApplicationStarted()
        {
            Logger.LogInformation("Application started. Use Exit menu to shut down");
            Logger.LogInformation("Hosting environment: {envName}", Environment.EnvironmentName);
            Logger.LogInformation("Content root path: {contentRoot}", Environment.ContentRootPath);
        }

        private void OnApplicationStopping()
        {
            Logger.LogInformation("Application is shutting down...");
        }

        private IHostApplicationLifetime HostApplicationLifetime { get; }
        private IHostEnvironment Environment { get; }
        private ILogger Logger { get; }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();
            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
        }
    }
}

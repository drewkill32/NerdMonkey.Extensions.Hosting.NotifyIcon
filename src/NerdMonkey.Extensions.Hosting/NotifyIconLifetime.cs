﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NerdMonkey.Extensions.Hosting
{
    public class NotifyIconLifetime:IHostLifetime,IDisposable
    {
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

        public NotifyIconLifetime(IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
        }
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _applicationStartedRegistration = ApplicationLifetime.ApplicationStarted.Register(state =>
                {
                    ((NotifyIconLifetime)state).OnApplicationStarted();
                },
                this);
            _applicationStoppingRegistration = ApplicationLifetime.ApplicationStopping.Register(state =>
                {
                    ((NotifyIconLifetime)state).OnApplicationStopping();
                },
                this);

            // systemd sends SIGTERM to stop the service.
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            return Task.CompletedTask;
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            ApplicationLifetime.StopApplication();

            _shutdownBlock.WaitOne();

            // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
            // Suppress that since we shut down gracefully. https://github.com/aspnet/AspNetCore/issues/6526
            System.Environment.ExitCode = 0;
        }

        private void OnApplicationStarted()
        {
            Logger.LogInformation("Application started. Press Ctrl+C to shut down.");
            Logger.LogInformation("Hosting environment: {envName}", Environment.EnvironmentName);
            Logger.LogInformation("Content root path: {contentRoot}", Environment.ContentRootPath);
        }

        private void OnApplicationStopping()
        {
            Logger.LogInformation("Application is shutting down...");
        }

        private IHostApplicationLifetime ApplicationLifetime { get; }
        private IHostEnvironment Environment { get; }
        private ILogger Logger { get; }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
        }
    }
}

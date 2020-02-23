using System;
using Microsoft.Extensions.DependencyInjection;
using NerdMonkey.Extensions.Hosting.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class NotfiyIconLifetimeHostBuilderExtensions
    {
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder) => builder.UseNotifyIcon(c => { });
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder, Action<NotifyIconOptions> configure)
        {
            if (!ApplicationHelper.IsDesktopApp())
            {
                return builder;
            }

            builder.ConfigureServices((hostContext, service) =>
            {
                service.AddSingleton<IHostLifetime, NotifyIconLifetime>();
                IconBuilder.Instance.ConfigureNotifyIcon(configure);
                service.AddSingleton(p=> IconBuilder.Instance.Build());
            });
            return builder;
        }
    }
}

using System;
using Microsoft.Extensions.DependencyInjection;
using NerdMonkey.App;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class NotfiyIconLifetimeHostBuilderExtensions
    {
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder) => builder.UseNotifyIcon(c => { });
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder, Action<NotifyIconOptions> configure)
        {
            builder.ConfigureServices((hostContext, service) =>
            {
                service.AddSingleton<IHostLifetime, NotifyIconLifetime>();
                service.Configure<NotifyIconOptions>(configure);
            });
            return builder;
        }
    }
}

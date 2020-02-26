using System;
using Microsoft.Extensions.DependencyInjection;
using NerdMonkey.Extensions.Hosting;
using NerdMonkey.Extensions.Hosting.NotifyIcon;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class NotfiyIconLifetimeHostBuilderExtensions
    {
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder) => builder.UseNotifyIcon(c => { });
        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder, Action<NotifyIconOptions> configure)=> builder.UseNotifyIcon((context, options) => configure(options));


        public static IHostBuilder UseNotifyIcon(this IHostBuilder builder, Action<HostBuilderContext,NotifyIconOptions> configure)
        {
            if (!ApplicationHelper.IsDesktopApp())
            {
                return builder;
            }

            builder.ConfigureServices((hostContext, service) =>
            {
                service.AddSingleton<IHostLifetime, FormApplicationLifetime>();
                IconBuilder.Instance.ConfigureNotifyIcon(options => configure(hostContext, options));
                service.Configure<NotifyIconOptions>(options => configure(hostContext, options));
                service.AddSingleton(p => IconBuilder.Instance.Build());
            });
            return builder;
        }
    }
}

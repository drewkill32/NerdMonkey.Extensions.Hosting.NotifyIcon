using System;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public interface INotifyIconBuilder
    {
        INotifyIcon Build();

        INotifyIconBuilder ConfigureNotifyIcon(Action<NotifyIconOptions> configure);
    }
}
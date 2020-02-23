using System;
using System.Collections.Generic;

namespace NerdMonkey.Extensions.Hosting.Configuration
{
    public interface INotifyIconBuilder
    {
        INotifyIconBuilder ConfigureNotifyIcon(Action<NotifyIconOptions> configure);
        INotifyIcon Build();
        INotifyIconBuilder RegisterNotifyIcon(Func<NotifyIconOptions, INotifyIcon> register);
    }


    public static class IconBuilder
    {
        public static INotifyIconBuilder Instance = new NotifyIconBuilder();
    }
    public class NotifyIconBuilder : INotifyIconBuilder
    {
        private readonly List<Action<NotifyIconOptions>> _configureNotifyTrayConfigActions = new List<Action<NotifyIconOptions>>();
        private readonly object _mutex = new object();
        private INotifyIcon _notifyIcon;
        private Func<NotifyIconOptions, INotifyIcon> _registeredNotifyIcon;
        public INotifyIconBuilder ConfigureNotifyIcon(Action<NotifyIconOptions> configure)
        {
            _configureNotifyTrayConfigActions.Add(configure);
            return this;
        }


        public NotifyIconBuilder()
        {
            _registeredNotifyIcon = options => new HostNotifyIcon(options);
        }
        public INotifyIconBuilder RegisterNotifyIcon(Func<NotifyIconOptions,INotifyIcon> register)
        {
            _registeredNotifyIcon = register;
            return this;
        }

        public INotifyIcon Build()
        {
            lock (_mutex)
            {
                if (_notifyIcon == null)
                {
                    var options = new NotifyIconOptions();
                    foreach (var configAction in _configureNotifyTrayConfigActions)
                    {
                        configAction(options);
                    }

                    _notifyIcon =_registeredNotifyIcon(options);
                }
            }

            return _notifyIcon;
        }
    }
}

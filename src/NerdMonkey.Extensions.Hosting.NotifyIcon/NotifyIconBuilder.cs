using System;
using System.Collections.Generic;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class NotifyIconBuilder : INotifyIconBuilder
    {
        private readonly List<Action<NotifyIconOptions>> _configureNotifyTrayConfigActions = new List<Action<NotifyIconOptions>>();
        private readonly object _mutex = new object();
        private INotifyIcon _notifyIcon;


        /// <summary>
        /// Returns an <see cref="INotifyIcon"/>. . will return the same instance if build has already been called.
        /// </summary>  
        /// <returns></returns>
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

                    _notifyIcon = new HostNotifyIcon(options);
                }
            }

            return _notifyIcon;
        }

        public INotifyIconBuilder ConfigureNotifyIcon(Action<NotifyIconOptions> configure)
        {
            _configureNotifyTrayConfigActions.Add(configure);
            return this;
        }
    }
}
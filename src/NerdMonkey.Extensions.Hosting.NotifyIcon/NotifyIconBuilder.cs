using System;
using System.Collections.Generic;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class NotifyIconBuilder : INotifyIconBuilder
    {
        private readonly List<Action<NotifyIconOptions>> _configureNotifyTrayConfigActions = new List<Action<NotifyIconOptions>>();
        private readonly object _mutex = new object();
        private bool _isBuilt;


        /// <summary>
        /// Returns an <see cref="INotifyIcon"/>. . will return the same instance if build has already been called.
        /// </summary>  
        /// <returns></returns>
        public INotifyIcon Build()
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }

            var options = new NotifyIconOptions();
            foreach (var configAction in _configureNotifyTrayConfigActions)
            {
                configAction(options);
            }
            NotifyIcon.InternalNotifyIcon = new HostNotifyIcon(options);
            _isBuilt = true;
            return NotifyIcon.InternalNotifyIcon;
        }

        public INotifyIconBuilder ConfigureNotifyIcon(Action<NotifyIconOptions> configure)
        {
            _configureNotifyTrayConfigActions.Add(configure);
            return this;
        }
    }
}
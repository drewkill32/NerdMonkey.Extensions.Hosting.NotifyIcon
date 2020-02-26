using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    internal class CompositeServiceProvider:IServiceProvider, IDisposable
    {
        private readonly IServiceProvider[] _providers;

        public CompositeServiceProvider(IEnumerable<IServiceProvider> providers)
        {
            _providers = providers.Reverse().ToArray();
        }

        public object GetService(Type serviceType)
        {
            foreach (var provider in _providers)
            {
                var service = provider.GetService(serviceType);
                if (service != null)
                {
                    return service;
                } 
            }
            return null;
        }

        public void Dispose()
        {
            foreach (var provider in _providers)
            {
                (provider as IDisposable)?.Dispose();
            }
        }
    }
}

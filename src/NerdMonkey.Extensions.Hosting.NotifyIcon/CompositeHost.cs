using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    internal class CompositeHost:IHost
    {
        private readonly IEnumerable<IHost> _hosts;

        public CompositeHost(IEnumerable<IHost> hosts)
        {
            _hosts = hosts;
            Services= new CompositeServiceProvider(_hosts.Select(h=> h.Services));
        }

        public void Dispose()
        {
            foreach (var host in _hosts)
            {
                host.Dispose();
            }
            (Services as IDisposable)?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var host in _hosts)
            {
                await host.StartAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var host in _hosts.Reverse())
            {
                await host.StopAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public IServiceProvider Services { get; }
    }
}

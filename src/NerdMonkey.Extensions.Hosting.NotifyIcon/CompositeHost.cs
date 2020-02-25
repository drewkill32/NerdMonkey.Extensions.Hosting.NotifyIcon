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
        private readonly INotifyIcon _notifyIcon;

        public CompositeHost(IEnumerable<IHost> hosts)
        {
            _hosts = hosts;
            Services= new CompositeServiceProvider(_hosts.Select(h=> h.Services));
            _notifyIcon = IconBuilder.Instance.Build();
            _notifyIcon.Exit += NotifyIcon_Exit;
        }

        public void Dispose()
        {
            _notifyIcon.Exit -= NotifyIcon_Exit;
            foreach (var host in _hosts)
            {
                host.Dispose();
            }
            _notifyIcon.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _notifyIcon.Show();
            foreach (var host in _hosts)
            {
                await host.StartAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var host in _hosts.Reverse())
            {
                await host.StopAsync(cancellationToken);
            }
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
            _notifyIcon.Hide();
        }

        public IServiceProvider Services { get; }
    }
}

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class HostedApplicationContext : ApplicationContext
    {
        private readonly IHost _host;
        private readonly INotifyIcon _notifyIcon;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _hostTask;

        /// <summary>
        /// Application Context used for NotifyIcon.
        /// </summary>
        /// <param name="hosts"></param>
        public HostedApplicationContext(IEnumerable<IHost> hosts)
        {
            _host = new CompositeHost(hosts);
            _notifyIcon = IconBuilder.Instance.Build();
            _notifyIcon.Exit += NotifyIcon_Exit;
            Run();
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
            _notifyIcon.Hide();
            _tokenSource.Cancel();
            _hostTask.GetAwaiter().GetResult();
            Application.Exit();
        }

        private void Run()
        {
            _notifyIcon.Show();
            _hostTask = _host.StartAsync(_tokenSource.Token);
        }

        protected override void Dispose(bool disposing)
        {
            _notifyIcon.Exit -= NotifyIcon_Exit;
            _notifyIcon.Dispose();
            _host.Dispose();
            base.Dispose(disposing);
        }
    }
}
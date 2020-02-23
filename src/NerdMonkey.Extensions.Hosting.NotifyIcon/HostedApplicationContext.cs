using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class HostedApplicationContext : ApplicationContext
    {
        private readonly IEnumerable<IHost> _hosts;
        private readonly INotifyIcon _notifyIcon;

        public HostedApplicationContext(IEnumerable<IHost> hosts)
        {
            _hosts = hosts;
            _notifyIcon = IconBuilder.Instance.Build();
            _notifyIcon.Exit += NotifyIcon_Exit;
            Run();
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
            _notifyIcon.Hide();
            Application.Exit();
        }

        private void Run()
        {
            _notifyIcon.Show();
            foreach (var host in _hosts)
            {
                new Thread(() => RunHost(host)).Start();
            }
        }

        private void RunHost(IHost host)
        {
            try
            {
                host.Run();
            }
            catch (OperationCanceledException)
            {
                //Ignore
            }
        }

        protected override void Dispose(bool disposing)
        {
            _notifyIcon.Exit -= NotifyIcon_Exit;
            _notifyIcon.Dispose();
            base.Dispose(disposing);
        }
    }
}
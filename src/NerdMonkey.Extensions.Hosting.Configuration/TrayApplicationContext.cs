using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace NerdMonkey.Extensions.Hosting.Configuration
{
    public class TrayApplicationContext : ApplicationContext
    {
        private readonly IEnumerable<IHost> _hosts;
        private readonly INotifyIcon _notifyIcon;

        public TrayApplicationContext(IEnumerable<IHost> hosts)
        {
            _hosts = hosts;
            _notifyIcon = IconBuilder.Instance.Build();
            _notifyIcon.Exit += NotifyIcon_Exit;
            Run();
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
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
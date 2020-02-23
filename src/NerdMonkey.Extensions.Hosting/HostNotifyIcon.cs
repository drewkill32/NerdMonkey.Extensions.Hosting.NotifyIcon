using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NerdMonkey.App
{
    public class HostNotifyIcon: IDisposable
    {
        private readonly NotifyIconOptions _options;
        private readonly NotifyIcon _notifyIcon;
        private ToolStripMenuItem _exitMenuItem;
        private ToolStripMenuItem _openUrlMenuItem;

        public event EventHandler Exit;

        public HostNotifyIcon(NotifyIconOptions options)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _options = options;
            _notifyIcon = new NotifyIcon()
            {
                Icon = _options.Icon,
                BalloonTipTitle = _options.Title,
                Text = _options.Title,
                Visible = true
            };
            BuildMenu();
        }

        private void BuildMenu()
        {

            _exitMenuItem = new ToolStripMenuItem(Resources.HostNotifyIcon_Exit, Resources.ExitImage);
            _exitMenuItem.Click += ExitOnClick;
            _openUrlMenuItem = new ToolStripMenuItem(_options.UrlMenuTitle, _options.Image);
            _openUrlMenuItem.Click += OpenUrlOnClick;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add(_openUrlMenuItem);
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            _notifyIcon.ContextMenuStrip.Items.Add(_exitMenuItem);
        }

        private void ExitOnClick(object sender, EventArgs e)
        {
            Hide();
            Exit?.Invoke(this, e);
        }

        private void OpenUrlOnClick(object sender, EventArgs e)
        {
            string url = _options.Url;
            OpenUrl(url);
        }

        private static void OpenUrl(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

        public void Dispose()
        {
            _exitMenuItem.Click -= ExitOnClick;
            _openUrlMenuItem.Click -= OpenUrlOnClick;
            _notifyIcon?.Dispose();
        }


        public void Hide()
        {
            _notifyIcon.Visible = false;
        }

        public void Show()
        {
            _notifyIcon.Visible = true;

            if (_options.DisplayStartupMessage)
            {
                _notifyIcon.BalloonTipText = _options.StartUpMessage;
                _notifyIcon.ShowBalloonTip(1000);
            }

            if (_options.OpenOnStartup)
            {
                OpenUrl(_options.Url);
            }
        }
    }
}

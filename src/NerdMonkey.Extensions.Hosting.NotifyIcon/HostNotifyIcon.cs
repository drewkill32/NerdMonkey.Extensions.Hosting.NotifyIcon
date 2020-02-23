using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public class HostNotifyIcon: INotifyIcon
    {
        private readonly NotifyIconOptions _options;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon;
        private ToolStripMenuItem _exitMenuItem;
        private ToolStripMenuItem _openUrlMenuItem;
        private bool _isShown;
        private readonly object _mutex = new object();
        public event EventHandler Exit;


        public HostNotifyIcon(NotifyIconOptions options)
        {
            _options = options;
            _notifyIcon = new System.Windows.Forms.NotifyIcon()
            {
                Icon = _options.Icon,
                BalloonTipTitle = _options.Title,
                Text = _options.Title,
                Visible = true
            };
            BuildMenu();
            _notifyIcon.DoubleClick += NotifyIconOnDoubleClick;
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

        private void NotifyIconOnDoubleClick(object sender, EventArgs e)
        {
            MethodInfo mi = typeof(System.Windows.Forms.NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(_notifyIcon, null);
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

            //has to be invoked this way in dotnet core https://github.com/dotnet/runtime/issues/17938
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
            _notifyIcon.DoubleClick -= NotifyIconOnDoubleClick;
            _notifyIcon?.Dispose();
        }


        public void Hide()
        {

            _notifyIcon.Visible = false;
            lock (_mutex)
            {
                _isShown = false;
            }
        }

        public void Show()
        {

            if (_isShown)
            {
                return;
            }

            lock (_mutex)
            {
                if (_isShown)
                {
                    return;
                }
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

                _isShown = true;
            }

        }
    }
}

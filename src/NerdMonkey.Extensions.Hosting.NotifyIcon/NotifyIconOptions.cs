using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    /// <summary>
    /// 
    /// </summary>
    public class NotifyIconOptions
    {
        private string _startUpMessage;
        private string _title;
        private string _urlMenuTitle;
        private bool _urlMenuTitleSet;
        private string _url;
        internal Action<ContextMenuStrip> ConfigureMenu { get; private set; }
        /// <summary>
        /// Icon used in System Tray
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Application Title
        /// </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_title))
                {
                    _title = Process.GetCurrentProcess().ProcessName;
                }

                return _title;
            }
            set => _title = value;
        }

        /// <summary>
        /// Display a message on startup
        /// </summary>
        public bool DisplayStartupMessage { get; set; }

        /// <summary>
        /// Opens the default web browser on startup
        /// </summary>
        public bool OpenOnStartup { get; set; }

        /// <summary>
        /// Message to be displayed on startup. Default: Starting <see cref="Title"/>
        /// </summary>
        public string StartUpMessage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_startUpMessage))
                {
                    _startUpMessage = string.Format(Resources.NotifyIconOptions_Starting, Title);
                }

                return _startUpMessage;
            }
            set => _startUpMessage = value;
        }

        /// <summary>
        /// Url of the web app. Default: http://localhost:5000
        /// </summary>
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                if (!_urlMenuTitleSet)
                {
                    _urlMenuTitle = string.Format(Resources.NotifyIconOptions_Open, Url);
                }
            }
        }

        /// <summary>
        /// Test to be displayed on 
        /// </summary>
        public string UrlMenuTitle
        {
            get => _urlMenuTitle;
            set
            {
                _urlMenuTitle = value;
                _urlMenuTitleSet = true;
            }
        }

        /// <summary>
        /// Action to add menu items to the context menu
        /// </summary>
        public void BuildMenu(Action<ContextMenuStrip> configure)
        {
            ConfigureMenu = configure ?? throw new ArgumentNullException(nameof(configure));
        }

        /// <summary>
        /// Image used in Menu
        /// </summary>
        public Image Image { get; set; }

        public NotifyIconOptions()
        {
            Url = "http://localhost:5000";
            _urlMenuTitle = string.Format(Resources.NotifyIconOptions_Open, Url);
            OpenOnStartup = true;
            DisplayStartupMessage = true;
            Icon = Resources.AppIcon;
            Image = Icon.ToBitmap();
            Url = "http://localhost:5000";
            ConfigureMenu = m => { };
        }
    }
}

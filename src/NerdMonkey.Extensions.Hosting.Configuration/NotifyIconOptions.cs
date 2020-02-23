using System.Diagnostics;
using System.Drawing;

namespace NerdMonkey.Extensions.Hosting.Configuration
{
    public class NotifyIconOptions
    {
        private string _startUpMessage;
        private string _title;
        private string _urlMenuTitle;
        private bool _urlMenuTitleSet;
        private string _url;
        public Icon Icon { get; set; }

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

        public bool DisplayStartupMessage { get; set; }

        public bool OpenOnStartup { get; set; }

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

        public string UrlMenuTitle
        {
            get => _urlMenuTitle;
            set
            {
                _urlMenuTitle = value;
                _urlMenuTitleSet = true;
            }
        }

        public Image Image { get; set; }

        public NotifyIconOptions()
        {
            Url = "http://localhost:5000";
            _urlMenuTitle = string.Format(Resources.NotifyIconOptions_Open, Url);
            OpenOnStartup = true;
            DisplayStartupMessage = true;
            Icon = Resources.AppIcon;
            Image = Resources.AppImage;
            Url = "http://localhost:5000";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NerdMonkey.Extensions.Hosting
{
    public class HostNotifyIcon
    {
        private NotifyIcon _notifyIcon;

        public HostNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
        }
    }
}

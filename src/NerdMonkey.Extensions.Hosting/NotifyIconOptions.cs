using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdMonkey.Extensions.Hosting
{
    public class NotifyIconOptions
    {
        public Icon Icon { get; set; }

        public string Title { get; set; }

        public bool DisplayStartupMessage { get; set; }

        public string StartUpMessage { get; set; }

        public Image Image { get; set; }

        public NotifyIconOptions()
        {
            StartUpMessage = $"Opening {Title}";
        }
    }
}

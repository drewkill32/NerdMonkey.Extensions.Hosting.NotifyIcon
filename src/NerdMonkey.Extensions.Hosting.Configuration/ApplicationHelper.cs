using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NerdMonkey.Extensions.Hosting.Configuration
{
    public static class ApplicationHelper
    {

        public static bool IsDesktopApp()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                return false;
            }

            var winFormAssembly =assembly.GetReferencedAssemblies().FirstOrDefault(a=> a.Name == "System.Windows.Forms");
            return winFormAssembly != null;
        }
    }
}

using System.Linq;
using System.Reflection;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
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

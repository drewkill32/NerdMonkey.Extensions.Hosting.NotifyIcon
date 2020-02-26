using System.Linq;
using System.Reflection;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public static class ApplicationHelper
    {
        /// <summary>
        /// Determines if the entry assembly is a desktop assembly
        /// by checking if it references System.Windows.Forms
        /// </summary>
        /// <returns>true if entry assembly references System.Windows.Forms</returns>
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

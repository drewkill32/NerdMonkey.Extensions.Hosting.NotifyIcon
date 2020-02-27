namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public static class NotifyIcon
    {
        private static INotifyIcon _internalNotifyIcon;
        public static INotifyIcon InternalNotifyIcon
        {
            get => _internalNotifyIcon;
            internal set => _internalNotifyIcon = value;
        } 
        public static INotifyIconBuilder Builder { get; }

        static NotifyIcon()
        {
            Builder= new NotifyIconBuilder();
        }
    }
}
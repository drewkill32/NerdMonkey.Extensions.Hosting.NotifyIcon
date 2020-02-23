using System;

namespace NerdMonkey.Extensions.Hosting.Configuration
{
    public interface INotifyIcon: IDisposable
    {
        void Hide();
        void Show();

        event EventHandler Exit;  
    }
}
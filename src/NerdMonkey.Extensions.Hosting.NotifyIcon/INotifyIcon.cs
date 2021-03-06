﻿using System;

namespace NerdMonkey.Extensions.Hosting.NotifyIcon
{
    public interface INotifyIcon: IDisposable
    {
        void Hide();
        void Show();

        event EventHandler Exit;  
    }
}
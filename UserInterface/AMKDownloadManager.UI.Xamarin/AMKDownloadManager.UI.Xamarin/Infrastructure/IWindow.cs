using System;
using System.ComponentModel;

namespace AMKDownloadManager.UI.Xamarin.Infrastructure
{
    public interface IWindow : IDisposable
    {
        event CancelEventHandler Closing;
        
        void Close();

        void Show();
        void Hide();

        void Maximize();
        void Minimize();
        
        bool IsTopMost { get; set; }
        
        string Title { get; set; }
    }
}
using System.ComponentModel;
using AMKDownloadManager.UI.Xamarin.Infrastructure;

namespace AMKDownloadManager.UI.Xamarin.GtkSharp.Impl
{
    public class GtkWindow : IWindow
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public event CancelEventHandler Closing;
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Maximize()
        {
            throw new System.NotImplementedException();
        }

        public void Minimize()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTopMost { get; set; }
        public string Title { get; set; }
    }
}
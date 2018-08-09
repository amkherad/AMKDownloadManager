using Xamarin.Forms;

namespace AMKDownloadManager.UI.Xamarin.Infrastructure
{
    public interface IWindowManager
    {
        IWindow CreateWindow(
            Page content,
            Image icon,
            string title,
            Size initialSize,
            Point initialPosition
            );
    }
}
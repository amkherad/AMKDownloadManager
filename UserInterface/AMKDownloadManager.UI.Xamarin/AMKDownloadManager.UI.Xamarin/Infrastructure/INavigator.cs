using Xamarin.Forms;

namespace AMKDownloadManager.UI.Xamarin.Infrastructure
{
    public interface INavigator
    {
        /// <summary>
        /// Return to previous page. - Not available in desktop.
        /// </summary>
        void GoBack();

        void Show(Page page);

        void ShowAndWaitForResult(View view);
    }
}
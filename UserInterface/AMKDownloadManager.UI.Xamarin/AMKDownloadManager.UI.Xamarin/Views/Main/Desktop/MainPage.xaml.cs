using System.Diagnostics;
using Xamarin.Forms;

namespace AMKDownloadManager.UI.Xamarin.Views.Main.Desktop
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContextChanged += (sender, args) =>
            {
                Trace.WriteLine("Test");
            };
        }
    }
}
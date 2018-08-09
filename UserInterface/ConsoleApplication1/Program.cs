using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Helpers;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GtkThemes.Init();

            if (PlatformHelper.GetGTKPlatform() == GTKPlatform.Windows)
                GtkThemes.LoadCustomTheme("Themes/gtkrc-dark");

            Gtk.Application.Init();
            Forms.Init();
            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Movies");
            window.SetApplicationIcon("Images/movies-icon.png");
            window.Show();
            Gtk.Application.Run();
        }
    }
}
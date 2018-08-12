using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Helpers;
using Xamarin.Forms.Xaml;

namespace AMKDownloadManager.UI.Xamarin.GtkSharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GtkThemes.Init();

            try
            {
               //if (PlatformHelper.GetGTKPlatform() == GTKPlatform.Windows)
                    GtkThemes.LoadCustomTheme("Themes/gtkrc-dark");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Gtk.Application.Init();
            Forms.Init();

            App app;
            if ((app = UIInitializer.CreateApp(args)) != null)
            {
                var window = new FormsWindow();
                window.LoadApplication(app);
                window.SetApplicationTitle("AMKDownloadManager");
                window.SetApplicationIcon("icon.png");
                window.Show();
                Gtk.Application.Run();
            }
        }
    }
}
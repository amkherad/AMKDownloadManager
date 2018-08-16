using AMKDownloadManager.UI.Xamarin.GtkSharp.Renderers;
using Gtk;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK.Renderers;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRendererGtkCompatible))]

namespace AMKDownloadManager.UI.Xamarin.GtkSharp.Renderers
{
    public class MasterDetailPageRendererGtkCompatible : MasterDetailPageRenderer
    {
        //override on
        protected override void OnShown()
        {
            Control.Toolbar.Direction = TextDirection.Ltr;
            
            var mb = new MenuBar();

            var menu = new Gtk.Menu();
            menu.Name = "File";
            
            var menuItem = new Gtk.MenuItem();
            menuItem.Name = "Test";
            
            menu.Add(menuItem);

            mb.Add(menu);
            
            Add(mb);
            
            base.OnShown();
        }
    }
}
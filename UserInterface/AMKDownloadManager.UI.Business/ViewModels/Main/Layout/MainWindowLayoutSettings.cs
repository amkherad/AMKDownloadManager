using System;
using System.Drawing;
using AMKDownloadManager.Core.Api;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.ViewModels.Main.Layout
{
    public class MainWindowLayoutSettings : NotifyPropertyChangedBase
    {
        public IApplicationContext AppContext { get; }
        
        public Point Position { get; set; }
        public Size Size { get; set; }
        
        public MainWindowLayoutSettings(IApplicationContext appContext)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            
            Size = new Size(500, 500);
        }
    }
}
using System.Diagnostics;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.UI.Business.Models;
using AMKDownloadManager.UI.Business.Models.Downloads;

namespace AMKDownloadManager.UI.Business.ViewModels.Downloads
{
    public class DownloadManagerContentViewModel : ViewModelBase
    {
        public IApplicationContext AppContext { get; }


        public string FilterText { get; set; } = "Ali";
        public DownloadCategoryItem FilterByCategory { get; set; }
        public DataGridDataModel DataGridData { get; set; }
        
        
        public DownloadManagerContentViewModel(IApplicationContext appContext)
        {
            AppContext = appContext;

            PropertyChanged += (sender, args) =>
            {
                Trace.WriteLine("Hello");
            };
        }
    }
}
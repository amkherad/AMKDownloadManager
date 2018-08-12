using System.Collections.ObjectModel;
using AMKDownloadManager.UI.Business.Models.Downloads;
using AMKDownloadManager.UI.Business.Services;

namespace AMKDownloadManager.UI.Business.ViewModels.Downloads
{
    public class DownloadCategoriesViewModel : ViewModelBase
    {
        public ObservableCollection<DownloadCategoryItem> Categories { get; }

        public DownloadCategoriesViewModel(IDownloadStateService stateService)
        {
            
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.Models
{
    public class DataGridDataModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<DataGridColumnModel> Columns { get; set; }
        
        public ObservableCollection<object> Rows { get; set; }
    }
}
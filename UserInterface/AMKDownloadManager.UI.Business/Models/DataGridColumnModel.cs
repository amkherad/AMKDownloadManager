using AMKsGear.Architecture;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.Models
{
    public class DataGridColumnModel : NotifyPropertyChangedBase
    {
        public string Title { get; set; }
        public SortingOrder SortingOrder { get; set; }
        public int ColumnWidth { get; set; }
        public bool Resizable { get; set; }
    }
}
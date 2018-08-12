using System.Collections.Generic;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.Models.Downloads
{
    public class DownloadCategoryItem : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DownloadCategoryItem Parent { get; set; }
        public IList<DownloadCategoryItem> Children { get; set; }


        public DownloadCategoryItem()
        {
            Children = new List<DownloadCategoryItem>();
        }

        public DownloadCategoryItem(IEnumerable<DownloadCategoryItem> children)
        {
            Children = new List<DownloadCategoryItem>(children);
        }
    }
}
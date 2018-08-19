using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AMKDownloadManager.UI.Business.Services
{
    public interface IDataLoader<TData>
    {
        IEnumerable<TData> Load();
        
        ObservableCollection<TData> LoadAndTrack();
    }
}
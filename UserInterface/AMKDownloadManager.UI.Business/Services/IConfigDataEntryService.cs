using AMKsGear.Core.Collections;

namespace AMKDownloadManager.UI.Business.Services
{
    public interface IConfigDataEntryService<TData> : IDataLoader<TData>, IObservableCollection<TData>
    {
        
    }
}
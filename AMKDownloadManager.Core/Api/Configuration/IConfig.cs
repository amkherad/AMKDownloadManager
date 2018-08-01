using System.ComponentModel;

namespace AMKDownloadManager.Core.Api.Configuration
{
    public interface IConfig : INotifyPropertyChanged, INotifyPropertyChanging
    {
        object GetValue();
        void SetValue(object value);
    }
}
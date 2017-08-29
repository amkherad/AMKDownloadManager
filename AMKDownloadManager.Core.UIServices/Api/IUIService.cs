using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.UIServices.Api
{
    public interface IUIService : IFeature
    {
        string Name { get; }
    }
}
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;

namespace AMKDownloadManager.Core.Api.Transport
{
    public interface IRequestTransport : IFeature
    {
        IResponse SendRequest(
            IAppContext appContext,
            DownloadItem downloadItem,
            IRequest request,
            bool unpackStream);
        
        Task<IResponse> SendRequestAsync(
            IAppContext appContext,
            DownloadItem downloadItem,
            IRequest request,
            bool unpackStream);
    }
}
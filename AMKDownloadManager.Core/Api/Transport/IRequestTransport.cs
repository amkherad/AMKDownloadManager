using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;

namespace AMKDownloadManager.Core.Api.Transport
{
    public interface IRequestTransport : IFeature
    {
        IResponse SendRequest(
            IApplicationContext applicationContext,
            DownloadItem downloadItem,
            IRequest request,
            bool unpackStream);
        
        Task<IResponse> SendRequestAsync(
            IApplicationContext applicationContext,
            DownloadItem downloadItem,
            IRequest request,
            bool unpackStream);
    }
}
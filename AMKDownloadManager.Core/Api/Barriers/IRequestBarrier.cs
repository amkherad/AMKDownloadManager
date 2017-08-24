using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public interface IRequestBarrier : IFeature
    {
        IResponse SendRequest(
            IAppContext appContext,
            IRequest request,
            IDownloadProgressListener downloadProgressListener,
            bool unpackStream);
        Task<IResponse> SendRequestAsync(
            IAppContext appContext,
            IRequest request,
            IDownloadProgressListener downloadProgressListener,
            bool unpackStream);
    }
}
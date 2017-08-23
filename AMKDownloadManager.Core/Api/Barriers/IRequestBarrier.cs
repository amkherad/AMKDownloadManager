using System.Threading.Tasks;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public interface IRequestBarrier : IFeature
    {
        IResponse SendRequest(IAppContext appContext, IRequest request);
        Task<IResponse> SendRequestAsync(IAppContext appContext, IRequest request);
    }
}
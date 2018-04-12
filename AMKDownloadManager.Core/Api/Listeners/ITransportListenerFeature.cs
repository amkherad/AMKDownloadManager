using System.IO;
using AMKDownloadManager.Core.Api.Transport;
using ir.amkdp.gear.arch.Annotations;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface ITransportListenerFeature : IListenerFeature
    {
        void BeforeSendRequest(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request);
        
        void WebRequestCreated(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest);
        
//        void HttpWebRequestCreated(
//            IAppContext appContext,
//            IRequestTransport transport,
//            IRequest request,
//            HttpWebRequest httpWebRequest);
        
        void WebBeforeRequestSubmission(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest);
        
        void WebResponseAvailable(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest,
            IResponse response,
            [CanBeNull] object webResponse,
            Stream responseStream);
    }
}
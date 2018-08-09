using System.IO;
using AMKDownloadManager.Core.Api.Transport;
using AMKsGear.Architecture.Annotations;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface ITransportListenerFeature : IListenerFeature
    {
        void BeforeSendRequest(
            IApplicationContext applicationContext,
            IRequestTransport transport,
            IRequest request);
        
        void WebRequestCreated(
            IApplicationContext applicationContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest);
        
//        void HttpWebRequestCreated(
//            IApplicationContext applicationContext,
//            IRequestTransport transport,
//            IRequest request,
//            HttpWebRequest httpWebRequest);
        
        void WebBeforeRequestSubmission(
            IApplicationContext applicationContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest);
        
        void WebResponseAvailable(
            IApplicationContext applicationContext,
            IRequestTransport transport,
            IRequest request,
            [CanBeNull] object webRequest,
            IResponse response,
            [CanBeNull] object webResponse,
            Stream responseStream);
    }
}
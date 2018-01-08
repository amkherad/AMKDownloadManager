using System.IO;
using System.Net;
using AMKDownloadManager.Core.Api.Transport;

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
            WebRequest webRequest);
        
//        void HttpWebRequestCreated(
//            IAppContext appContext,
//            IRequestTransport transport,
//            IRequest request,
//            HttpWebRequest httpWebRequest);
        
        void WebBeforeRequestSubmission(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request,
            WebRequest webRequest);
        
        void WebResponseAvailable(
            IAppContext appContext,
            IRequestTransport transport,
            IRequest request,
            WebRequest webRequest,
            IResponse response,
            WebResponse webResponse,
            Stream responseStream);
    }
}
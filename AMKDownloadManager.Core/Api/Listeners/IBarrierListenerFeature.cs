using System.IO;
using System.Net;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IBarrierListenerFeature : IListenerFeature
    {
        void BeforeSendRequest(
            IAppContext appContext,
            IRequestBarrier barrier,
            IRequest request);
        
        void WebRequestCreated(
            IAppContext appContext,
            IRequestBarrier barrier,
            IRequest request,
            WebRequest webRequest);
        
//        void HttpWebRequestCreated(
//            IAppContext appContext,
//            IRequestBarrier barrier,
//            IRequest request,
//            HttpWebRequest httpWebRequest);
        
        void WebBeforeRequestSubmission(
            IAppContext appContext,
            IRequestBarrier barrier,
            IRequest request,
            WebRequest webRequest);
        
        void WebResponseAvailable(
            IAppContext appContext,
            IRequestBarrier barrier,
            IRequest request,
            WebRequest webRequest,
            IResponse response,
            WebResponse webResponse,
            Stream responseStream);
    }
}
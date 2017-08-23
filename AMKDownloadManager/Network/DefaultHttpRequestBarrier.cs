using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Network
{
    public class DefaultHttpRequestBarrier : IHttpRequestBarrier
    {
        public IResponse SendRequest(IAppContext appContext, IRequest request)
        {
            var task = SendRequestAsync(appContext, request);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<IResponse> SendRequestAsync(IAppContext appContext, IRequest request)
        {
            var listeners = appContext.GetFeatures<IBarrierListenerFeature>().ToList();
            listeners.ForEach(l => l.BeforeSendRequest(appContext, this, request));

            var webRequest = HttpHelpers.CreateHttpWebRequestFromRequest(request);
            listeners.ForEach(l => l.WebRequestCreated(appContext, this, request, webRequest));

            using (var stream = await webRequest.GetRequestStreamAsync())
            {
                var requestBody = request.RequestBody;
                if (requestBody != null)
                {
                    stream.Write(requestBody, 0, requestBody.Length);
                }

                request.RequestBodyWriter?.Invoke(stream);

                stream.Close();
            }

            listeners.ForEach(l => l.WebBeforeRequestSubmission(appContext, this, request, webRequest));
            try
            {
                using (var webResponse = await webRequest.GetResponseAsync() as HttpWebResponse)
                {
                    if (webResponse != null)
                    {
                        if (webRequest.HaveResponse)
                        {
                            using (var stream = webResponse.GetResponseStream())
                            {
                                var response = HttpHelpers.CreateResponseFromHttpResponse(webResponse, stream);
                                
                                listeners.ForEach(l =>
                                    l.WebResponseAvailable(appContext, this, request, webRequest,
                                        response, webResponse, stream));

                                return response;
                            }
                        }
                        else
                        {
                            var response = HttpHelpers.CreateResponseFromHttpResponse(webResponse);
                            
                            listeners.ForEach(l =>
                                l.WebResponseAvailable(appContext, this, request, webRequest,
                                    response, webResponse, null));
                            
                            return response;
                        }
                    }
                    
                    throw new InvalidOperationException();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse) wex.Response)
                    {
                        var responseStream = errorResponse.GetResponseStream();
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                string error = reader.ReadToEnd();
                                //TODO: use JSON.net to parse this string and look at the error message
                            }
                        }
                    }
                }
                throw;
            }
        }

//        foreach (var ip in Dns.GetHostAddresses (Dns.GetHostName ())) 
//        {
//            Console.WriteLine ("Request from: " + ip);
//            var request = (HttpWebRequest)HttpWebRequest.Create ("http://ns1.vianett.no/");
//            request.ServicePoint.BindIPEndPointDelegate = delegate {
//                return new IPEndPoint (ip, 0);
//            };
//            var response = (HttpWebResponse)request.GetResponse ();
//            Console.WriteLine ("Actual IP: " + response.GetResponseHeader ("X-YourIP"));
//            response.Close ();
//        }
        public int Order => 0;
    }
}
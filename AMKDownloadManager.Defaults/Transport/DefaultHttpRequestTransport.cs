using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.Defaults.Network;
using ir.amkdp.gear.core.Trace;

namespace AMKDownloadManager.Defaults.Transport
{
    public class DefaultHttpRequestTransport : IHttpTransport
    {
        private int _maxRedirects = KnownConfigs.DownloadManager.Download.MaximumRedirectsDefaultValue;

        // ReSharper disable once InconsistentNaming
        protected virtual IPEndPoint BindIPEndPoint(
            IAppContext appContext,
            DownloadItem downloadItem,
            ServicePoint servicePoint,
            IPEndPoint remoteEndPoint,
            int retryCount)
        {
            var selector = appContext.GetFeature<INetworkInterfaceSelector>();

            var endPoint = selector.SelectInterface(appContext, this, downloadItem);
            if (endPoint == null)
            {
                throw new NetworkInterfaceException();
            }

            if (endPoint is IPEndPoint)
            {
                return endPoint as IPEndPoint;
            }
            if (endPoint is NetworkInterface)
            {
#warning Use all available unicasts if one does not working.
                var firstAddress = (endPoint as NetworkInterface).GetIPProperties().UnicastAddresses.FirstOrDefault();
                if (firstAddress == null)
                {
                    throw new NetworkInterfaceException();
                }

                return new IPEndPoint(firstAddress.Address, 0);
            }
            if (endPoint is NetworkInterfaceInfo)
            {
                var ni = NetworkInterfaceProvider.GetNetworkInterfaceByName((endPoint as NetworkInterfaceInfo).Name);
                if (ni == null)
                {
                    throw new NetworkInterfaceException();
                }

#warning Use all available unicasts if one does not working.
                var inAdd = ni.GetIPProperties().UnicastAddresses.FirstOrDefault();
                if (inAdd == null)
                {
                    throw new NetworkInterfaceException();
                }

                return new IPEndPoint(inAdd.Address, 0);
            }

            throw new NetworkInterfaceException();
        }

        //[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public IResponse SendRequest(
            IAppContext appContext,
            DownloadItem downloadItem,
            IRequest request,
            IDownloadProgressListener downloadProgressListener,
            bool unpackStream)
        {
            appContext.SignalFeatures<ITransportListenerFeature>(
                l => l.BeforeSendRequest(appContext, this, request));

            var webRequest = HttpHelpers.CreateHttpWebRequestFromRequest(
                request,
                new HashSet<string>
                {
                    "Referer",
                    "User-Agent"
                });
            appContext.SignalFeatures<ITransportListenerFeature>(
                l => l.WebRequestCreated(appContext, this, request, webRequest));

            var referer = request.Headers.Referer;
            if (referer != null) webRequest.Referer = referer;

            var userAgent = request.Headers.UserAgent;
            if (userAgent != null) webRequest.UserAgent = userAgent;

            #region DownloadItem properties

            var proxy = downloadItem.HttpProxy;
            if (proxy != null && proxy.Uri != null)
            {
                webRequest.Proxy =
                    proxy.BypassList == null
                        ? new WebProxy(proxy.Uri, proxy.BypassOnLocal)
                        : new WebProxy(proxy.Uri, proxy.BypassOnLocal, proxy.BypassList.ToArray());
            }
            var iface = downloadItem.Interface;
            if (iface != null)
            {
                webRequest.ServicePoint.BindIPEndPointDelegate = (servicePoint, remoteEndPoint, retryCount)
                    => BindIPEndPoint(
                        appContext,
                        downloadItem,
                        servicePoint,
                        remoteEndPoint,
                        retryCount);
            }

            #endregion

            var requestBody = request.RequestBody;
            var requestBodyWriter = request.RequestBodyWriter;
            if (!request.IsGetRequest() && (requestBody != null || requestBodyWriter != null))
            {
                using (var stream = webRequest.GetRequestStream())
                {
                    if (requestBody != null)
                    {
                        stream.Write(requestBody, 0, requestBody.Length);
                    }

                    requestBodyWriter?.Invoke(stream);

                    stream.Close();
                }
            }

            //webRequest.KeepAlive = false;
            webRequest.AllowAutoRedirect = true;
            webRequest.MaximumAutomaticRedirections = _maxRedirects;
            //webRequest.all
            //webRequest.AllowReadStreamBuffering = false;

            //var receiveBufferSize = webRequest.ServicePoint.ReceiveBufferSize;

            appContext.SignalFeatures<ITransportListenerFeature>(
                l => l.WebBeforeRequestSubmission(appContext, this, request, webRequest));
            try
            {
                var webResponse = webRequest.GetResponse() as HttpWebResponse;
                {
                    if (webResponse != null)
                    {
                        if (webResponse.ResponseUri != webRequest.RequestUri)
                        {
                            downloadItem.Redirect(webResponse.ResponseUri);
                        }

                        if (webRequest.HaveResponse)
                        {
                            var stream = webResponse.GetResponseStream();
                            {
                                //webResponse.Close();
                                //var data = (new BinaryReader(stream)).ReadBytes((int)stream.Length);
                                //File.WriteAllBytes("out.bin", data);

                                var response = new HttpResponse();
                                HttpHelpers.FillResponseFromHttpResponse(response, webResponse, stream);

                                response.Disposer.Enqueue(
                                    stream,
                                    webResponse
                                );

                                //appContext.SignalFeatures<ITransportListenerFeature>(
                                //    l => l.WebResponseAvailable(appContext, this, request, webRequest,
                                //        response, webResponse, stream));

                                //return response;
                                return response;
                            }
                        }
                        else
                        {
                            var response = new HttpResponse();
                            HttpHelpers.FillResponseFromHttpResponse(response, webResponse);

                            appContext.SignalFeatures<ITransportListenerFeature>(
                                l => l.WebResponseAvailable(appContext, this, request, webRequest,
                                    response, webResponse, null));

                            return response;
                        }
                    }

                    throw new InvalidOperationException();
                }
            }
            catch (WebException wex)
            {
                Logger.Write(wex);
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

        public async Task<IResponse> SendRequestAsync(
            IAppContext appContext,
            DownloadItem downloadItem,
            IRequest request,
            IDownloadProgressListener downloadProgressListener,
            bool unpackStream)
        {
            throw new NotImplementedException();
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

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Download.MaximumRedirects))
            {
                _maxRedirects = configProvider.GetInt(this,
                    KnownConfigs.DownloadManager.Download.MaximumRedirects,
                    KnownConfigs.DownloadManager.Download.MaximumRedirectsDefaultValue
                );
            }
        }
    }
}
using System;
using System.Net;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;
using AMKsGear.Core.Text;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJobPart : IJobPart
    {
        public IAppContext AppContext { get; }

        public IJob Job { get; }
        public IHttpTransport Transport { get; protected set; }
        
        public IFileManager FileManager { get; }
        public DownloadItem DownloadItem { get; }
        
        public HttpProtocolProvider ProtocolProvider { get; }
        
        private readonly SegmentationContext _segmentation;

        private INetworkMonitor _networkMonitor;

        private readonly Segment _segment;

        private readonly IDownloadProgressListener _progressListener;
        
        public HttpDownloadJobPart(
            IAppContext appContext,
            DownloadItem downloadItem,
            HttpProtocolProvider protocolProvider,
            IJob job,
            IFileManager fileManager,
            SegmentationContext segmentationContext,
            Segment segment,
            IDownloadProgressListener progressListener)
        {
            AppContext = appContext;

            FileManager = fileManager;
            ProtocolProvider = protocolProvider;

            DownloadItem = downloadItem;
            
            Job = job;
            _segmentation = segmentationContext;

            _segment = segment;
            _progressListener = progressListener;

            _networkMonitor = appContext.GetFeature<INetworkMonitor>();
        }

        #region IJobPart implementation

        public JobPartState Cycle()
        {
            if (Transport == null)
            {
                Transport = AppContext.GetFeature<IHttpTransport>();
				//#error rename all IHttpRequestTransport to IHttpTransport
            }
            
            var request = ProtocolProvider.CreateRequest(
                AppContext,
                DownloadItem,
                _segmentation,
                _segment
            );
            var response = Transport.SendRequest(AppContext, DownloadItem, request, _progressListener, false);
			//#error error handling for prev. line
            
            if (response.StatusCode == HttpStatusCode.PartialContent)
            {
                var contentRange = response.Headers.ContentRange;
                if (contentRange != null)
                {
                    contentRange = contentRange.Trim();
                    //Content-Range: bytes 236040-59445247/59445248
                    const string Bytes = "bytes";
                    if (contentRange.StartsWith(Bytes))
                    {
                        contentRange = contentRange.Substring(Bytes.Length);

                        var slashParts = contentRange.Split('/');
                        var minMax = slashParts[0].Split('-');
                        var min = minMax[0].ToInt64();
                        var max = minMax[1].ToInt64();

                        var length = response.ResponseStream.Length;
                        if (length != max - min + 1)
                        {
                            return JobPartState.ErrorCanRetry;
                        }
                        
                        var stream = response.ResponseStream;

                        var fileSaver = AppContext.GetFeature<IStreamSaver>();

//                        FileManager.SaveStream(
//                            response.ResponseStream,
//                            min,
//                            0,
//                            length
//                        );
                        try
                        {
                            fileSaver.SaveStream(
                                stream,
                                FileManager,
                                _segmentation,
                                0,
                                null,
                                _defaultBufferSize,
                                _limit
                            );
                    
                            return JobPartState.Finished;
                        }
                        catch (Exception ex)
                        {
                            return JobPartState.ErrorCanRetry;
                        }
                        
                        return JobPartState.Finished;
                    }
                }
                else
                {
                    return JobPartState.ErrorCanRetry;
                }
            }
            else if (response.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
            {
                //https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/416
                //TODO: MDN: download will be considered as non-resumable or ask for the whole document again.
                return JobPartState.Error;
				//#error {{download will be considered as non-resumable or ask for the whole document again.}} why JobPartState.Error ??
            }
            else
            {
                return JobPartState.ErrorCanRetry;
            }

            return JobPartState.ErrorCanRetry;
        }

        public void NotifyAbort()
        {
            
        }

        public void Dispose()
        {
            
        }

        #endregion
    }
}
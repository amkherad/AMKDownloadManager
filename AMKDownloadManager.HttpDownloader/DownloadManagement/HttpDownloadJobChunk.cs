using System;
using System.Net;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;
using ir.amkdp.gear.core.Text;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJobChunk : IJobChunk
    {
        public IAppContext AppContext { get; }

        public IJob Job { get; }
        public IHttpRequestBarrier Barrier { get; protected set; }
        
        public IFileManager FileManager { get; }
        public DownloadItem DownloadItem { get; }
        
        public HttpProtocolProvider ProtocolProvider { get; }
        
        private SegmentationContext _segmentation;

        private INetworkMonitor _networkMonitor;

        private Segment _segment;

        private IDownloadProgressListener _progressListener;
        
        public HttpDownloadJobChunk(
            IAppContext appContext,
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
            
            Job = job;
            _segmentation = segmentationContext;

            _segment = segment;
            _progressListener = progressListener;

            _networkMonitor = appContext.GetFeature<INetworkMonitor>();
        }

        #region IJobChunk implementation

        public JobChunkState Cycle()
        {
            if (Barrier == null)
            {
                Barrier = AppContext.GetFeature<IHttpRequestBarrier>();
            }
            
            var request = ProtocolProvider.CreateRequest(
                AppContext,
                DownloadItem,
                _segmentation,
                _segment
            );
            var response = Barrier.SendRequest(AppContext, DownloadItem, request, _progressListener, false);
            
            if (response.StatusCode == HttpStatusCode.PartialContent)
            {
                var contentRange = response.Headers.ContentRange;
                if (contentRange != null)
                {
                    contentRange = contentRange.Trim();
                    //Content-Range: bytes 236040-59445247/59445248
                    const string Bytes = "bytes ";
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
                            return JobChunkState.ErrorCanTry;
                        }
                        
                        FileManager.SaveStream(
                            response.ResponseStream,
                            min,
                            0,
                            length
                        );
                        
                        return JobChunkState.Finished;
                    }
                }
                else
                {
                    return JobChunkState.ErrorCanTry;
                }
            }
            else if (response.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
            {
                //https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/416
                //TODO: MDN: download will be considered as non-resumable or ask for the whole document again.
                return JobChunkState.Error;
            }
            else
            {
                return JobChunkState.ErrorCanTry;
            }

            return JobChunkState.ErrorCanTry;
        }

        public void Yield()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
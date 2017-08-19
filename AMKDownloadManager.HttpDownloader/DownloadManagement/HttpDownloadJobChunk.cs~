using System;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJobChunk : IJobChunk
    {
        public IAppContext AppContext { get; }
        public IRequest Request { get; }

        private INetworkMonitor _networkMonitor;
        
        public HttpDownloadJobChunk(
            IAppContext appContext,
            IRequest request)
        {
            AppContext = appContext;
            Request = request;

            _networkMonitor = appContext.GetFeature<INetworkMonitor>();
        }

        #region IJobChunk implementation

        public JobChunkState Cycle()
        {
            
        }

        public void Yield()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
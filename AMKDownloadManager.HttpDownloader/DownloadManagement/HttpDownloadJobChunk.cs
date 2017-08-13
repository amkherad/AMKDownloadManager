using System;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJobChunk : IJobChunk
    {
        public HttpDownloadJobChunk()
        {
        }

        #region IJobChunk implementation

        public JobChunkState Cycle()
        {
            throw new NotImplementedException();
        }

        public void Yield()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
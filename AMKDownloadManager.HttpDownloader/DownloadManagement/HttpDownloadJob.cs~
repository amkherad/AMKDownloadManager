using System;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJob : IJob
    {
        public IAppContext AppContext { get; }
        public DownloadItem DownloadItem { get; }

        public HttpDownloadJob(
            IAppContext appContext,
            DownloadItem downloadItem)
        {
            AppContext = appContext;
            DownloadItem = downloadItem;
        }

        #region IJob implementation

        public event EventHandler Finished;

        public event EventHandler Progress;

        public event EventHandler Paused;

        public event EventHandler Started;


        protected void OnFinished(EventArgs e) => Finished?.Invoke(this, e);
        protected void OnProgress(EventArgs e) => Progress?.Invoke(this, e);
        protected void OnPaused(EventArgs e) => Paused?.Invoke(this, e);
        protected void OnStarted(EventArgs e) => Started?.Invoke(this, e);


        public IJobChunk StartChunk()
        {
            var request = new HttpRequest(DownloadItem);

            var chunk = new HttpDownloadJobChunk(AppContext, request);
            return chunk;
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Clean()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFeature implementation

        public int Order => 0;

        #endregion
    }
}
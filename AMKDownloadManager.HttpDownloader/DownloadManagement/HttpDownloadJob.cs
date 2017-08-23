using System;
using System.Net;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJob : IJob
    {
        public IAppContext AppContext { get; }
        
        public HttpProtocolProvider HttpProtocolProvider { get; }
        
        public DownloadItem DownloadItem { get; }
        public JobParameters JobParameters { get; }

        public IHttpRequestBarrier Barrier { get; protected set; }
        
        
        
        public HttpDownloadJob(
            IAppContext appContext,
            HttpProtocolProvider httpProtocolProvider,
            DownloadItem downloadItem,
            JobParameters jobParameters)
        {
            AppContext = appContext;
            HttpProtocolProvider = httpProtocolProvider;
            
            DownloadItem = downloadItem;
            JobParameters = jobParameters;
        }

        #region IJob implementation

        public event JobEventHandler Finished;
        public event JobEventHandler Paused;
        public event JobEventHandler Started;
        public event JobProgressEventHandler Progress;
        public event JobPriorityChangedEventHandler PriorityChanged;

        protected void OnFinished(EventArgs e) => Finished?.Invoke(this, e);
        protected void OnProgress(int progress) => Progress?.Invoke(this, progress);
        protected void OnPaused(EventArgs e) => Paused?.Invoke(this, e);
        protected void OnStarted(EventArgs e) => Started?.Invoke(this, e);


        public JobInfo TriggerJobAndGetInfo()
        {
            if (Barrier == null)
            {
                Barrier = AppContext.GetFeature<IHttpRequestBarrier>();
            }
            var jobInfo = new JobInfo();

            var request = HttpProtocolProvider.CreateRequest(AppContext, DownloadItem);
            var response = Barrier.SendRequest(AppContext, request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var accept = response.Headers.Accept;
                if (accept != null)
                {
                    
                }
            }
            
            return jobInfo;
        }

        public Task<JobInfo> TriggerJobAndGetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public IJobChunk GetJobChunk()
        {
            throw new NotImplementedException();
        }

        public void Clean()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFeature implementation

        public int Order => 0;

        #endregion
    }
}
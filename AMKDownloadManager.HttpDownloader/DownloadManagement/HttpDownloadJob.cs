using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
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
        public IJobDivider SegmentProvider { get; protected set; }

        public IFileManager FileManager { get; protected set; }
        
        private readonly ProgressListener _progressListener;


        private long? _resourceSize;
        private SegmentationContext _segmentation;
        
        
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

            Barrier = appContext.GetFeature<IHttpRequestBarrier>();
            SegmentProvider = appContext.GetFeature<IJobDivider>();
            
            _progressListener = new ProgressListener();
        }

        #region IJob implementation

        public event JobEventHandler Finished;
        public event JobEventHandler Paused;
        public event JobEventHandler Started;
        public event JobProgressEventHandler Progress;
        public event JobPriorityChangedEventHandler PriorityChanged;
        public event JobStateChangedEventHandler StateChanged;

        public void OnFinished(EventArgs e) => Finished?.Invoke(this, e);
        public void OnProgress(long progress) => Progress?.Invoke(this, progress);
        public void OnPaused(EventArgs e) => Paused?.Invoke(this, e);
        public void OnStarted(EventArgs e) => Started?.Invoke(this, e);
        public void OnPriorityChanged(SchedulerPriority priority) => PriorityChanged?.Invoke(this, priority);
        public void OnStateChanged(JobState state) => StateChanged?.Invoke(this, state);


        public JobState State { get; protected internal set; }
        
        public JobInfo TriggerJobAndGetInfo()
        {
            if (Barrier == null)
            {
                Barrier = AppContext.GetFeature<IHttpRequestBarrier>();
            }
            bool supportsConcurrency = false;
            long? downloadSize;

            var request = HttpProtocolProvider.CreateRequest(AppContext, DownloadItem);
            var response = Barrier.SendRequest(AppContext, request, _progressListener, false);

            downloadSize = response.Headers.ContentLength;

            if (downloadSize != null)
            {
                
            }
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var accept = response.Headers.AcceptRanges;
                if (accept != null)
                {
                    supportsConcurrency = accept == "bytes";
                }
            }
            
            return new JobInfo(downloadSize, supportsConcurrency, response);
        }

        public Task<JobInfo> TriggerJobAndGetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public IJobChunk GetJobChunk(JobInfo jobInfo)
        {
            
        }

        public void BindService<T>(T service)
        {
            if (typeof(T) == typeof(IFileManager))
            {
                FileManager = (IFileManager) service;
            }
            else
            {
                throw new InvalidOperationException("Unknown service.");
            }
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
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider)
        {
            
        }

        #endregion

        private class ProgressListener : IDownloadProgressListener
        {
            public void OnProgress(
                IAppContext appContext,
                IJob job,
                DownloadItem downloadItem,
                IHttpRequestBarrier barrier,
                long totalSize,
                long progress)
            {
                var j = job as HttpDownloadJob;
                j?.OnProgress(progress);
            }

            public void OnFinished(
                IAppContext appContext,
                IJob job,
                DownloadItem downloadItem,
                IHttpRequestBarrier barrier)
            {
                var j = job as HttpDownloadJob;
                j?.OnFinished(EventArgs.Empty);
            }
        }
    }
}
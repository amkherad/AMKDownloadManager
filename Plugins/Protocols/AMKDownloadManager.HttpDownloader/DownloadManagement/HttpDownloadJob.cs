using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;
using AMKsGear.Core.Automation;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpDownloadJob : IJob
    {
        public IAppContext AppContext { get; }

        public HttpProtocolProvider HttpProtocolProvider { get; }

        //public IRequest Request { get; }
        public DownloadItem DownloadItem { get; }

        public JobParameters JobParameters { get; }

        public IHttpTransport Transport { get; protected set; }
        public ISegmentDivider SegmentProvider { get; protected set; }

        public IFileManager FileManager { get; protected set; }


        internal long? ResourceSize;
        internal SegmentationContext Segmentation;
        internal Segment InitialSegment;

        private long _defaultBufferSize;
        private long _minSegmentSize;
        private long _maxSegmentSize;

        public HttpDownloadJob(
            IAppContext appContext,
            IFileManager fileManager,
            HttpProtocolProvider httpProtocolProvider,
            DownloadItem downloadItem,
            JobParameters jobParameters)
        {
            AppContext = appContext;
            HttpProtocolProvider = httpProtocolProvider;

            FileManager = fileManager;

            DownloadItem = downloadItem;
            JobParameters = jobParameters;

            Transport = appContext.GetFeature<IHttpTransport>();
            SegmentProvider = appContext.GetFeature<ISegmentDivider>();

            LoadConfig(appContext, appContext.GetFeature<IConfigProvider>(), null);
        }

        #region IJob implementation

        public JobState State { get; protected internal set; }

        public JobInfo TriggerJobAndGetInfo()
        {
            if (Transport == null)
            {
                Transport = AppContext.GetFeature<IHttpTransport>();
            }
            bool supportsConcurrency = false;
            long? downloadSize;
            //bool isFinished = false;

            IResponse response;
            using (var request = HttpProtocolProvider.CreateRequest(
                AppContext,
                DownloadItem,
                null,
                null,
                null
            ))
            {
                response = Transport.SendRequest(
                    AppContext,
                    DownloadItem,
                    request,
                    false
                );
            }

            downloadSize = response.Headers.ContentLength;

            var segment = new Segment(0, _minSegmentSize);
            if (downloadSize != null)
            {
                ResourceSize = downloadSize;
                Segmentation = new SegmentationContext(downloadSize.Value);
                InitialSegment = segment;
                Segmentation.ReservedRanges.Add(segment);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var accept = response.Headers.AcceptRanges;
                if (accept != null)
                {
                    //readStream = false;
                    supportsConcurrency = accept == "bytes";
                }
            }

            IJobPart mainJobPart = null;
            if ((downloadSize ?? 0) > 0)
            {
                mainJobPart = new MainJobPartImpl(
                    AppContext,
                    this,
                    FileManager,
                    Segmentation,
                    segment,
                    response
                );
            }

            var jobInfo = new JobInfo(
                supportsConcurrency ? null : downloadSize,
                downloadSize,
                supportsConcurrency,
                response,
                //isFinished,
                mainJobPart,
                Segmentation);

            jobInfo.Disposer.Enqueue(response);
            
            return jobInfo;
        }
        
        public Task<JobInfo> TriggerJobAndGetInfoAsync()
        {
            return Task.Run(() => TriggerJobAndGetInfo());
        }

        private class MainJobPartImpl : IJobPart
        {
            public IAppContext AppContext { get; }
            public HttpDownloadJob Job { get; }
            public IFileManager FileManager { get; }
            public IResponse Response { get; }
            
            public SegmentationContext SegmentationContext { get; }
            public Segment Segment { get; }

            private long? _limit;
            private long _defaultBufferSize;

            IJob IJobPart.Job => Job; 
            
            public MainJobPartImpl(
                IAppContext appContext,
                HttpDownloadJob job,
                IFileManager fileManager,
                SegmentationContext segmentationContext,
                Segment segment,
                IResponse response)
            {
                AppContext = appContext;
                Job = job;
                FileManager = fileManager;
                SegmentationContext = segmentationContext;
                Response = response;

                Segment = segment;
                
                //_limit = job._defaultBufferSize;
                _defaultBufferSize = job._defaultBufferSize;
            }
            
            public JobPartState Cycle()
            {
                var stream = Response.ResponseStream;
                
                var fileSaver = AppContext.GetFeature<IStreamSaver>();

                try
                {
                    fileSaver.SaveStream(
                        stream,
                        FileManager,
                        SegmentationContext,
                        Segment,
                        _defaultBufferSize,
                        _limit
                    );
                    
                    return JobPartState.Finished;
                }
                catch (Exception ex)
                {
                    return JobPartState.ErrorCanRetry;
                }
            }

            public void NotifyAbort()
            {
                
            }

            public void Dispose()
            {
                Response.Dispose();
            }
            
            
#if DEBUG
            public string DebugName { get; set; }
        
            public string GetDebugName()
            {
                return DebugName;
            }
#endif
        }

        public IJobPart GetJobPart(JobInfo jobInfo)
        {
            if (Segmentation == null)
            {
                throw new InvalidOperationException();
            }
            if (FileManager == null)
            {
                throw new InvalidOperationException();
            }
            
            var partDescriptor = SegmentProvider.GetPart(
                AppContext,
                this,
                Segmentation
            );

            if (partDescriptor == null)
            {
                return null;
            }

            return new HttpDownloadJobPart(
                AppContext,
                DownloadItem,
                HttpProtocolProvider,
                this,
                FileManager,
                Segmentation,
                partDescriptor.Segment,
                _defaultBufferSize
            );
        }

        public void Clean()
        {
            
        }

        public void Reset()
        {
            
        }
        
        #if DEBUG
        public string GetDebugName()
        {
            return DownloadItem.GetDebugName();
        }
        #endif

        public void Dispose()
        {
            
        }

        #endregion

        #region IFeature implementation

        //public int Order => 0;

        private void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSize))
            {
                _defaultBufferSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSize,
                    KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSizeDefaultValue
                );
            }
            
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MinSegmentSize))
            {
                _minSegmentSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue
                );
            }
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize))
            {
                _maxSegmentSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSizeDefaultValue
                );
            }
        }

        #endregion
    }
}
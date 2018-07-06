﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;
using AMKsGear.Core.Automation;

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

        private readonly ProgressListener _progressListener;


        internal long? ResourceSize;
        internal SegmentationContext Segmentation;

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

            _progressListener = new ProgressListener();

            LoadConfig(appContext, appContext.GetFeature<IConfigProvider>(), null);
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
            if (Transport == null)
            {
                Transport = AppContext.GetFeature<IHttpTransport>();
            }
            bool supportsConcurrency = false;
            long? downloadSize;
            //bool isFinished = false;

            var request = HttpProtocolProvider.CreateRequest(
                AppContext,
                DownloadItem,
                null,
                null
            );

            var response = Transport.SendRequest(
                AppContext,
                DownloadItem,
                request,
                _progressListener,
                false
            );

            downloadSize = response.Headers.ContentLength;

            if (downloadSize != null)
            {
                ResourceSize = downloadSize;
                Segmentation = new SegmentationContext(downloadSize.Value);
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
                    response
                );
            }

            var jobInfo = new JobInfo(
                supportsConcurrency ? null : downloadSize,
                downloadSize,
                supportsConcurrency,
                response,
                //isFinished,
                mainJobPart);

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

            private long? _limit;
            private long _defaultBufferSize;
            
            public MainJobPartImpl(
                IAppContext appContext,
                HttpDownloadJob job,
                IFileManager fileManager,
                SegmentationContext segmentationContext,
                IResponse response)
            {
                AppContext = appContext;
                Job = job;
                FileManager = fileManager;
                SegmentationContext = segmentationContext;
                Response = response;

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
            }

            public void NotifyAbort()
            {
                
            }

            public void Dispose()
            {
                Response.Dispose();
            }
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
                _progressListener
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

        private class ProgressListener : IDownloadProgressListener
        {
            public void OnProgress(
                IAppContext appContext,
                IJob job,
                DownloadItem downloadItem,
                IHttpTransport transport,
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
                IHttpTransport transport)
            {
                var j = job as HttpDownloadJob;
                j?.OnFinished(EventArgs.Empty);
            }
        }
    }
}
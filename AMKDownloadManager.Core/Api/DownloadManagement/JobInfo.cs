using System;
using AMKDownloadManager.Core.Api.Transport;
using AMKsGear.Core.Automation;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public class JobInfo : IDisposable 
    {
        /// <summary>
        /// Gets the size of the download if available.
        /// </summary>
        /// <value>The size of the download.</value>
        public long? DownloadSize { get; }

        /// <summary>
        /// Gets the size of the first request content if available.
        /// </summary>
        public long? FirstHttpPacketSize { get; }
        
        /// <summary>
        /// Main job part to continue job on a diffrent download manager thread.
        /// </summary>
        public IJobPart MainJobPart { get; } 
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="AMKDownloadManager.Core.Api.DownloadManagement.IJob"/>
        /// supports concurrency.
        /// </summary>
        /// <value><c>true</c> if supports concurrency; otherwise, <c>false</c>.</value>
        public bool SupportsConcurrency { get; }

        /// <summary>
        /// Gets response of first request. if available.
        /// </summary>
        public IResponse Response { get; }

        public IDisposer Disposer { get; }
        
        /// <summary>
        /// Gets a value indicating whether this job is finished. typically becomes true when job does not support concurrency. (resume capability)
        /// </summary>
        //public bool IsFinished { get; set; }
        
        public SegmentationContext SegmentationContext { get; }

        /// <summary>
        /// JobInfo constructor
        /// </summary>
        /// <param name="downloadSize">Size of download resource, (if available)</param>
        /// <param name="firstHttpPacketSize">Size of first request's response content</param>
        /// <param name="supportsConcurrency">Determines download resource supports concurrency (resume capability)</param>
        /// <param name="response">IResponse if available</param>
        /// <param name="mainJobPart"></param>
        public JobInfo(
            long? downloadSize,
            long? firstHttpPacketSize,
            bool supportsConcurrency,
            IResponse response,
            //bool isFinished,
            IJobPart mainJobPart,
            SegmentationContext segmentationContext)
        {
            Disposer = new Disposer();
            
            DownloadSize = downloadSize;
            FirstHttpPacketSize = firstHttpPacketSize;
            SupportsConcurrency = supportsConcurrency;
            Response = response;
            //IsFinished = isFinished;
            MainJobPart = mainJobPart;
            SegmentationContext = segmentationContext;
        }

        public void Dispose()
        {
            Disposer.Dispose();
        }
    }
}
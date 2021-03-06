﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.Core.Api.Types;
using AMKDownloadManager.HttpDownloader.DownloadManagement;
using AMKsGear.Architecture.Annotations;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.HttpDownloader.ProtocolProvider
{
    public class HttpProtocolProvider : IProtocolProvider
    {
        public static string[] SupportedProtocols =
        {
            "http",
            "https"
        };

        private string _defaultMethod = "GET";

        public HttpProtocolProvider()
        {
        }

        #region IProtocolProvider implementation

        public bool CanHandle(IApplicationContext applicationContext, DownloadItem downloadItem)
        {
            if (downloadItem == null)
                throw new ArgumentNullException(nameof(downloadItem));

            var uri = downloadItem.Uri;
            if (uri == null)
            {
                return false;
            }

            var scheme = uri.Scheme;
            if (string.IsNullOrWhiteSpace(scheme) || SupportedProtocols.Any(p => p == scheme.ToLower()))
            {
                return true;
            }

            return false;
        }

        public IRequest CreateRequest(
            [NotNull] IApplicationContext applicationContext,
            [NotNull] DownloadItem downloadItem,
            [CanBeNull] SegmentationContext segmentationContext,
            [CanBeNull] Segment segment,
            [CanBeNull] RequestParameters parameters)
        {
            var request = HttpRequest.FromDownloadItem(
                applicationContext,
                downloadItem);

            var isRangedRequest = false;

            if (segment != null && segmentationContext != null)
            {
                isRangedRequest = true;
                if (segment.LimitedSegment)
                {
                    request.Headers.Range = $"bytes={segment.Min}-{segment.Max}";
                }
                else
                {
                    request.Headers.Range = $"bytes={segment.Min}-{segmentationContext.TotalSize}";
                }
            }

            if (parameters != null)
            {
                if (!parameters.SuppressConsistencyCheck)
                {
                    var dateTime = parameters.ConsistencyDateTime;
                    var eTag = parameters.ConsistencyEntityTag;

                    if (isRangedRequest)
                    {
                        if (eTag != null && !string.IsNullOrWhiteSpace(eTag))
                        {
                            request.Headers.IfRange = eTag;
                        }
                        else if (dateTime != null && !string.IsNullOrWhiteSpace(dateTime))
                        {
                            request.Headers.IfRange = dateTime;
                        }
                    }
#warning IfMatch/IfUnmodifiedSince suppressed when Range header is presented.
                    else if (eTag != null && !string.IsNullOrWhiteSpace(eTag))
                    {
                        request.Headers.IfMatch = eTag;
                    }
                    else if (dateTime != null && !string.IsNullOrWhiteSpace(dateTime))
                    {
                        request.Headers.IfUnmodifiedSince = dateTime;
                    }
                }
            }

            applicationContext.SignalFeatures<IProtocolProviderListener>(x => x.RequestCreated(
                applicationContext,
                downloadItem,
                request,
                this
            ));

            request.Method = _defaultMethod;

            return request;
        }

        public IJob CreateJob(
            IApplicationContext applicationContext,
            DownloadItem downloadItem,
            IFileProvider fileProvider,
            JobParameters jobParameters)
        {
            var fileManager = fileProvider.CreateFile(
                applicationContext,
                downloadItem.LocalFileName ??
                (downloadItem.Uri == null ? null : Path.GetFileName(downloadItem.Uri.AbsolutePath)),
                null,
                null,
                null
            );
            
            var httpDownload = new HttpDownloadJob(
                applicationContext,
                fileManager,
                this,
                downloadItem,
                jobParameters
            );

            applicationContext.SignalFeatures<IProtocolProviderListener>(x => x.JobCreated(
                applicationContext,
                downloadItem,
                httpDownload,
                jobParameters,
                this
            ));

            return httpDownload;
        }

        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }

        #endregion
    }
}
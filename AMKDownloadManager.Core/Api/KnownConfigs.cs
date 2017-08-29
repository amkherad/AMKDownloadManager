using System;
using ir.amkdp.gear.core.Utils;

namespace AMKDownloadManager.Core.Api
{
    public static class KnownConfigs
    {
        public static class DownloadManager
        {
            public static class Download
            {
                public const string MaxSimultaneousConnections = "DownloadManager.Download.MaxSimultaneousConnections";
                public const int MaxSimultaneousConnections_DefaultValue = 8;

                public const string MaxSimultaneousJobs = "DownloadManager.Download.MaxSimultaneousJobs";
                public const int MaxSimultaneousJobs_DefaultValue = 3;

                public const string MaxRetries = "DownloadManager.Download.MaxRetries";
                public const int MaxRetries_DefaultValue = 5;

                public const string RetryDelay = "DownloadManager.Download.RetryDelay";
                public const int RetryDelay_DefaultValue = 500;

                public const string RequestMethod = "DownloadManager.Download.DefaultRequestMethod";
                public const string RequestMethod_DefaultValue = "GET";
            }

            public static class Segmentation
            {
                public const string MinSegmentSize = "DownloadManager.Segmentation.MinSegmentSize";
                public const int MinSegmentSize_DefaultValue = 4 * Helper.KiB;
                
                public const string MaxSegmentSize = "DownloadManager.Segmentation.MaxSegmentSize";
                public const int MaxSegmentSize_DefaultValue = 10 * Helper.MiB;
            }
        }

    }
}
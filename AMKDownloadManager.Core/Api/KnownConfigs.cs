using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Core.Api
{
    public static class KnownConfigs
    {
        public static class DownloadManager
        {
            public static class Download
            {
                public const string MaxSimultaneousConnections = "DownloadManager.Download.MaxSimultaneousConnections";
                public const int MaxSimultaneousConnectionsDefaultValue = 8;

                public const string MaxSimultaneousJobs = "DownloadManager.Download.MaxSimultaneousJobs";
                public const int MaxSimultaneousJobsDefaultValue = 3;

                public const string MaxRetries = "DownloadManager.Download.MaxRetries";
                public const int MaxRetriesDefaultValue = 5;

                public const string RetryDelay = "DownloadManager.Download.RetryDelay";
                public const int RetryDelayDefaultValue = 500;

                public const string RequestMethod = "DownloadManager.Download.DefaultRequestMethod";
                public const string RequestMethodDefaultValue = "GET";

                public const string MaximumRedirects = "DownloadManager.Download.MaximumRedirects";
                public const int MaximumRedirectsDefaultValue = 20;

                public const string DefaultReceiveBufferSize = "DownloadManager.Download.DefaultReceiveBufferSize";
                public const int DefaultReceiveBufferSizeDefaultValue = 4 * Helper.KiB;
            }

            public static class Segmentation
            {
                public const string MinSegmentSize = "DownloadManager.Segmentation.MinSegmentSize";
                public const int MinSegmentSizeDefaultValue = 4 * Helper.KiB;
                
                public const string MaxSegmentSize = "DownloadManager.Segmentation.MaxSegmentSize";
                public const int MaxSegmentSizeDefaultValue = 10 * Helper.MiB;
            }
        }

        public static class FileSystem
        {
            public const string DownloadLocation = "FileSystem.DownloadLocation";
            public const string DownloadLocationDefaultValue = "[Downloads]";

            public const string TempDownloadLocation = "FileSystem.TempDownloadLocation";
            public const string TempDownloadLocationDefaultValue = "[Downloads]";

            public const string UseTempLocation = "FileSystem.UseTempLocation";
            public const bool UseTempLocationDefaultValue = false;
                
            public const string DuplicityResolvationStart = "FileSystem.DuplicityResolvationStart";
            public const int DuplicityResolvationStartDefaultValue = 2;
        }
    }
}
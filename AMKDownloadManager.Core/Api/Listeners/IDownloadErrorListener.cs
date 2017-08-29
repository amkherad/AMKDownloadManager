using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadErrorListener : IFeature
    {
        void OnDeadError(
            IAppContext appContext,
            IJob job,
            IDownloadManager downloadManager
        );
        
        void OnGetInfoError(
            IAppContext appContext,
            IJob job,
            IDownloadManager downloadManager,
            bool retrying
        );

        void OnChunkError(
            IAppContext appContext,
            IJob job,
            IJobChunk jobChunk,
            IDownloadManager downloadManager,
            bool retrying
        );
    }
}
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadErrorListener : IListenerFeature
    {
        void OnDeadError(
            IApplicationContext applicationContext,
            IJob job,
            IDownloadManager downloadManager
        );
        
        void OnGetInfoError(
            IApplicationContext applicationContext,
            IJob job,
            IDownloadManager downloadManager,
            bool retrying
        );

        void OnPartError(
            IApplicationContext applicationContext,
            IJob job,
            IJobPart jobPart,
            IDownloadManager downloadManager,
            bool retrying
        );
    }
}
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadErrorListener : IListenerFeature
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

        void OnPartError(
            IAppContext appContext,
            IJob job,
            IJobPart jobPart,
            IDownloadManager downloadManager,
            bool retrying
        );
    }
}
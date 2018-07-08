using System;
using System.Threading.Tasks;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Download manager service.
    /// </summary>
    public interface IDownloadManager : IFeature
    {
        /// <summary>
        /// Begins a download job.
        /// </summary>
        /// <param name="job">Job.</param>
        IDownloadManagerHandle Schedule(IJob job);

        /// <summary>
        /// Begins a download job and call the source when the download job finishes.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="job">Job.</param>
        /// <param name="callback">Callback.</param>
        IDownloadManagerHandle Schedule(IJob job, Action<IJob> callback);


        /// <summary>
        /// Starts download manager jobs.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops download manager jobs.
        /// </summary>
        void Stop();
        /// <summary>
        /// Force stop download manager jobs.
        /// </summary>
        void ForceStop();

        void WaitToFinish();
        void WaitToFinish(TimeSpan timeout);
        
        void WaitToFinish(IJob job);
        void WaitToFinish(IJob job, TimeSpan timeout);
    }
}
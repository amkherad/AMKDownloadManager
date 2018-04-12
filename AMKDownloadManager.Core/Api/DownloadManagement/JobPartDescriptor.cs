namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public class JobPartDescriptor
    {
        public IJob Job { get; }
        public IJobPart JobPart { get; }

        public JobPartDescriptor(IJob job, IJobPart jobPart)
        {
            Job = job;
            JobPart = jobPart;
        }
    }
}
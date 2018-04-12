namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface IJobDivider : IFeature
    {
        PartialBlockDescriptor GetPart(
            IAppContext appContext,
            IJob job, 
            SegmentationContext segmentationContext);
    }
}
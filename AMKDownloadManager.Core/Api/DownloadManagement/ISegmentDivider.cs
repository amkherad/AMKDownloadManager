namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface ISegmentDivider : IFeature
    {
        PartialBlockDescriptor GetPart(
            IAppContext appContext,
            IJob job,
            SegmentationContext segmentationContext);
        
        PartialBlockDescriptor GetPart(
            IAppContext appContext,
            IJob job,
            SegmentationContext segmentationContext,
            long contiguousLeftOffset);
    }
}
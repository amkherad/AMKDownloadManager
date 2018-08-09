namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface ISegmentDivider : IFeature
    {
        PartialBlockDescriptor GetPart(
            IApplicationContext applicationContext,
            IJob job,
            SegmentationContext segmentationContext);
        
        PartialBlockDescriptor GetPart(
            IApplicationContext applicationContext,
            IJob job,
            SegmentationContext segmentationContext,
            long contiguousLeftOffset);
    }
}
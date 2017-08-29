namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface IJobDivider : IFeature
    {
        ChunkDescriptor GetChunk(
            IAppContext appContext,
            IJob job, 
            SegmentationContext segmentationContext);
    }
}
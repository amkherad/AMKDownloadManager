namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public class ChunkDescriptor
    {
        public Segment Segment { get; }
        
        public ChunkDescriptor(Segment segment)
        {
            Segment = segment;
        }
    }
}
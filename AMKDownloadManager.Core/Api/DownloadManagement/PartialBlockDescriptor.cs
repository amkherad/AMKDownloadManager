namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public class PartialBlockDescriptor
    {
        public Segment Segment { get; }
        
        public PartialBlockDescriptor(Segment segment)
        {
            Segment = segment;
        }
    }
}
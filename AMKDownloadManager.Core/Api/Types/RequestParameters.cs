namespace AMKDownloadManager.Core.Api.Types
{
    public class RequestParameters
    {
        public string ConsistencyEntityTag { get; set; }
        public string ConsistencyDateTime { get; set; }
        
        public bool SuppressConsistencyCheck { get; set; }
        
        
    }
}
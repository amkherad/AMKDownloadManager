namespace AMKDownloadManager.Core.Api.Messaging
{
    /// <summary>
    /// The enum used to specify message source.
    /// </summary>
    public enum MessageSource
    {
        /// <summary>
        /// Specifies that message came from somewhere inside current process.
        /// </summary>
        LocalProcess,
        
        /// <summary>
        /// Specifies that message came from outside of process boundaries (inter-process - using IPC)
        /// </summary>
        InterProcess
    }
}
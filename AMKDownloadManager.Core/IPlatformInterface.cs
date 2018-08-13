namespace AMKDownloadManager.Core
{
    public interface IPlatformInterface
    {
        void SignalReceived(ApplicationSignals signal);
    }
}
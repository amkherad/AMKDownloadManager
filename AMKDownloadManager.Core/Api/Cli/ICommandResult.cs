namespace AMKDownloadManager.Core.Api.Cli
{
    public interface ICommandResult
    {
        string GetStandardOutput();
        string GetDebugOutput();
        string GetWarningOutput();
        string GetErrorOutput();
    }
}
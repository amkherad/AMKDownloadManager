namespace AMKDownloadManager.Core.Api.Cli
{
    public interface ICliInterface
    {
        void Write(string text);
        void WriteLine(string text);

        string ReadLine();
        char ReadChar();
    }
}
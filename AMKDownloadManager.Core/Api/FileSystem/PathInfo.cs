namespace AMKDownloadManager.Core.Api.FileSystem
{
    public class PathInfo
    {
        public string Path { get; }
        public string TempPath { get; }
        
        public bool UseTemp { get; }

        public PathInfo(string path, string tempPath, bool useTemp)
        {
            Path = path;
            TempPath = tempPath;
            UseTemp = useTemp;
        }
    }
}
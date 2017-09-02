using System.IO;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultFileManager : IFileManager
    {
        public void SaveStream(Stream stream, long fileStart, long streamStart, long length)
        {
            throw new System.NotImplementedException();
        }

        public void SaveBinary(byte[] binary, long fileStart, long binaryStart, long length)
        {
            throw new System.NotImplementedException();
        }

        public void Move(string newPath)
        {
            throw new System.NotImplementedException();
        }
    }
}
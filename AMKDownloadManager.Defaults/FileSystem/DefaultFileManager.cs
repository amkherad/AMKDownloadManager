using System;
using System.Diagnostics;
using System.IO;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public enum FileManagerLockMode
    {
        NoLock = 0,
        LockOnEdit = 1,
        LockOnProgress = 2
    }

    public class DefaultFileManager : IFileManager
    {
        /// <summary>
        /// Path of the file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Optional file mime type.
        /// </summary>
        public string MimeType { get; set; }

#warning FileManagerLockMode not used in the code.
        /// <summary>
        /// Determines <see cref="FileManagerLockMode"/> for the file.
        /// </summary>
        public FileManagerLockMode LockMode { get; set; }

        private object _lock = new object();

        /// <summary>
        /// <see cref="DefaultFileManager"/> constructor.
        /// </summary>
        /// <param name="path">Path to the file</param>
        public DefaultFileManager(string path)
        {
            Path = path;
        }

        public void InitFile()
        {
            using (var f = new FileStream(Path, FileMode.OpenOrCreate))
                f.Close();
        }

        public void SaveStream(Stream stream, long fileStart, long streamStart, long length)
        {
            if (streamStart > 0)
            {
                if (stream.CanSeek)
                {
                    stream.Position = streamStart;
                }
                else
                {
                    throw new NotSupportedException("Unseekable stream does not supported.");
                }
            }

            using (var f = new FileStream(Path,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, (int) length))
            {
                f.Seek(fileStart, SeekOrigin.Begin);
                using (var binary = new BinaryReader(stream))
                {
                    f.Write(binary.ReadBytes((int) length), (int) streamStart, (int) length);
                }
            }
        }

        public void SaveBinary(byte[] binary, long fileStart, long binaryStart, long length)
        {
            using (var f = new FileStream(Path,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, (int) length))
            {
                f.Seek(fileStart, SeekOrigin.Begin);
                f.Write(binary, (int) binaryStart, (int) length);
            }
        }

        public void Move(string newPath)
        {
            File.Move(Path, newPath);
            Path = newPath;
        }

        public bool Exists()
        {
            return File.Exists(Path);
        }

        public void Dispose()
        {
#warning Used as a part of FileManagerLockMode implementation.
        }
    }
}
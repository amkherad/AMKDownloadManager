using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultProgressiveStreamSaver : IStreamSaver
    {
        public void SaveStream(
            Stream stream,
            IFileManager fileManager,
            long start,
            long? end,
            long? partsSize,
            long? limit)
        {
            if (partsSize != null)
            {
                int partSizeLng = (int) partsSize.Value;
                var offset = start;
                var part = new byte[partSizeLng];
                long length = stream.Read(part, 0, partSizeLng);
                long totalRead = 0L;
                try
                {
                    while (length > 0)
                    {
                        fileManager.SaveBinary(part, offset, 0, length);
                        offset += length;
                        totalRead += length;

                        if (limit != null && totalRead >= limit.Value)
                        {
                            break;
                        }

                        length = stream.Read(part, 0, partSizeLng);
                    }
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Debugger.Break();
                }
            }
            else
            {
                fileManager.SaveStream(stream, start, 0, stream.Length);
            }
        }
        
        #region DefaultProgressiveStreamSaver
        public int Order => 0;
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
        #endregion
    }
}
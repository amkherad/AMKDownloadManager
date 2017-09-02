using System;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultFileProvider : IFileProvider
    {
        public IFileManager CreateFile(IAppContext appContext, string name, long? size, int? chunks)
        {
            var pathProvider = appContext.GetFeature<IDownloadPathProvider>();

            if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }
            
            //pathProvider.GetPathForMedia()
            return null;
        }

        public string TryReserveFileName(string name)
        {
            throw new System.NotImplementedException();
        }

        public int Order => 0;
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider)
        {
            
        }
    }
}
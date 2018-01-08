using System;
using System.Collections.Generic;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;
using ir.amkdp.gear.core.Utils;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultFileProvider : IFileProvider
    {
        public int DuplicityResolvationStart { get; set; }

        public DefaultFileProvider()
        {
            DuplicityResolvationStart = 1;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duplicityResolvationStart">Duplicity resolvation appendix number start. (default = 1)</param>
        public DefaultFileProvider(int duplicityResolvationStart)
        {
            DuplicityResolvationStart = duplicityResolvationStart;
        }

        public IFileManager CreateFile(
            IAppContext appContext,
            string name,
            string mimeType,
            long? size,
            int? chunks)
        {
            var pathProvider = appContext.GetFeature<IDownloadPathProvider>();

            if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }

            string media = null;
            if (mimeType != null)
            {
                try
                {
                    media = Helper.GetMimeTypeFromString(mimeType).Media;
                }
                catch
                {
                }
            }

            var pathInfo = pathProvider.GetPathForMedia(appContext, media, name);

            var path = TryReserveFileName(
                Path.Combine(pathInfo.UseTemp ? pathInfo.TempPath : pathInfo.Path, name)
            );

            //pathProvider.GetPathForMedia()

            var result = new DefaultFileManager(path);
            result.InitFile();
            
            return result;
        }
        
        public IFileManager ResumeFile(
            IAppContext appContext,
            string filePath,
            string mimeType,
            long? size,
            int? chunks)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            return new DefaultFileManager(filePath);
        }

        public string TryReserveFileName(string name)
        {
            var fileName = Path.GetFileNameWithoutExtension(name);
            var path = Path.GetDirectoryName(name);
            var fileExtension = Path.GetExtension(name);

            var counter = DuplicityResolvationStart;
            while (Directory.Exists(name) || File.Exists(name))
            {
                name = Path.Combine(path, $"{fileName}_{counter++}{fileExtension}");
            }

            return name;
        }

        public int Order => 0;

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
        }
    }
}
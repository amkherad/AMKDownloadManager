using System;
using System.Collections.Generic;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Defaults.Messaging;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultFileProvider : IFileProvider
    {
        public IApplicationContext AppContext { get; set; }
        
        public int DuplicityResolutionStart { get; set; }

        
        public DefaultFileProvider(IApplicationContext appContext)
        {
            AppContext = appContext;
            DuplicityResolutionStart = 1;
        }
        
        public IFileManager CreateFile(
            IApplicationContext applicationContext,
            string name,
            string mimeType,
            long? size,
            int? parts)
        {
            var pathProvider = applicationContext.GetFeature<IDownloadPathProvider>();

            if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }

            string media = null;
            if (mimeType != null)
            {
                //MimeType.TryParse(mimeType, out media);
                #warning UNCOMMENT THIS!
            }

            var pathInfo = pathProvider.GetPathForMedia(applicationContext, media, name);

            var path = TryReserveFileName(
                Path.Combine(pathInfo.UseTemp ? pathInfo.TempPath : pathInfo.Path, name)
            );
            //pathProvider.GetPathForMedia()

            var result = new DefaultFileManager(path);
            result.InitFile();
            
            return result;
        }
        
        public IFileManager ResumeFile(
            IApplicationContext applicationContext,
            string filePath,
            string mimeType,
            long? size,
            int? parts)
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

            var counter = DuplicityResolutionStart;
            while (Directory.Exists(name) || File.Exists(name))
            {
                name = Path.Combine(path, $"{fileName}_{counter++}{fileExtension}");
            }

            return name;
        }

        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultDownloadPathProvider : IDownloadPathProvider
    {
        public PathInfo GetPathForMedia(
            IApplicationContext applicationContext,
            string mediaType,
            string fileName)
        {
            var config = applicationContext.GetFeature<IConfigProvider>();

            var defaultDownloadLocation = config.GetString(this,
                KnownConfigs.FileSystem.DownloadLocation,
                KnownConfigs.FileSystem.DownloadLocationDefaultValue);

            var tempDownloadLocation = config.GetString(this,
                KnownConfigs.FileSystem.DownloadLocation,
                KnownConfigs.FileSystem.DownloadLocationDefaultValue);

            var useTempLocation = config.GetBool(this,
                KnownConfigs.FileSystem.UseTempLocation,
                KnownConfigs.FileSystem.UseTempLocationDefaultValue);

            if (tempDownloadLocation != null)
            {
                tempDownloadLocation = ReplacePath(tempDownloadLocation);
            }

            return new PathInfo(
                ReplacePath(defaultDownloadLocation),
                tempDownloadLocation,
                useTempLocation && tempDownloadLocation != null
            );
        }

        public string ReplacePath(string path)
        {
            const string Downloads = "[Downloads]";
            const string Documents = "[Documents]";
            const string Musics = "[Musics]";
            const string Videos = "[Videos]";
            const string Pictures = "[Pictures]";
            const string Applications = "[Applications]";
            const string AppData = "[AppData]";

            int index;
            
            if ((index = path.IndexOf(Downloads, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Downloads.Length),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads"));
            }
            if ((index = path.IndexOf(Applications, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Applications.Length),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Applications"));
            }
            if ((index = path.IndexOf(Documents, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Documents.Length),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            if ((index = path.IndexOf(Musics, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Musics.Length),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            }
            if ((index = path.IndexOf(Videos, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Videos.Length),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            }
            if ((index = path.IndexOf(Pictures, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, Pictures.Length),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            }
            if ((index = path.IndexOf(AppData, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                path = path.Replace(path.Substring(index, AppData.Length),
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }

            return path;
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
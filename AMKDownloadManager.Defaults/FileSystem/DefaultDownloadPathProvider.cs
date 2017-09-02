using System;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultDownloadPathProvider : IDownloadPathProvider
    {
        public PathInfo GetPathForMedia(
            IAppContext appContext,
            string mediaType,
            string fileName)
        {
            var config = appContext.GetFeature<IConfigProvider>();

            var defaultDownloadLocation = config.GetString(this,
                KnownConfigs.DownloadManager.Download.DownloadLocation,
                KnownConfigs.DownloadManager.Download.DownloadLocationDefaultValue);

            var tempDownloadLocation = config.GetString(this,
                KnownConfigs.DownloadManager.Download.DownloadLocation,
                KnownConfigs.DownloadManager.Download.DownloadLocationDefaultValue);

            var useTempLocation = config.GetBool(this,
                KnownConfigs.DownloadManager.Download.UseTempLocation,
                KnownConfigs.DownloadManager.Download.UseTempLocationDefaultValue);

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
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider)
        {
            
        }
    }
}
using AMKDownloadManager.Core.Api;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core
{
    public class AppContext : AppCrossCuttingContext, IAppContext
    {
        public const string ApplicationProfileDirectoryName = "AMKDownloadManager";
        public const string PluginRepositoryEnvironmentVariableName = "AMKDM_PLUGIN_REPOSITORY";
    }
}
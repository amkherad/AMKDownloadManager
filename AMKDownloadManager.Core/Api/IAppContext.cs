using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application service pool.
    /// </summary>
    public interface IAppContext : IStorageAppContext, ITypeResolverAppContext
    {
        
    }
}
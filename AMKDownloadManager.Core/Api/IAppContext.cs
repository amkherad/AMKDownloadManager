using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application service pool.
    /// </summary>
    public interface IApplicationContext : IStorageAppContext, ITypeResolverAppContext
    {
        
    }
}
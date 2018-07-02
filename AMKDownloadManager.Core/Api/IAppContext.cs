using System;
using AMKsGear.Architecture.Patterns;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application service pool.
    /// </summary>
    public interface IAppContext : ICrossCuttingContext, IStorageCrossCuttingContext
    {
        
    }
}
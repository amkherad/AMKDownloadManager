using System;
using ir.amkdp.gear.arch.Patterns;
using ir.amkdp.gear.core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application service pool.
    /// </summary>
    public interface IAppContext : ICrossCuttingContext, IStorageCrossCuttingContext
    {
        
    }
}
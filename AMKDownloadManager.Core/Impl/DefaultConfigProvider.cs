using System;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.Impl
{
    public class DefaultConfigProvider : IConfigProvider
    {
        public DefaultConfigProvider()
        {
        }

        #region IFeature implementation

        public int Order => 0;

        #endregion
    }
}
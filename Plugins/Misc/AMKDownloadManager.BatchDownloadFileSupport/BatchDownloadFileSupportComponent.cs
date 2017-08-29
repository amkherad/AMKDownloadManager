using System;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Extensions;

namespace AMKDownloadManager.BatchDownloadFileSupport
{
    public class BatchDownloadFileSupportComponent : IComponent
    {
        public string Name { get; }
        public string Description { get; }
        public string Author { get; }
        public Version Version { get; }
        public void Install(IAppContext app)
        {
            throw new NotImplementedException();
        }

        public void Uninstall(IAppContext app)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IAppContext app)
        {
            throw new NotImplementedException();
        }

        public void Unload(IAppContext app)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Composition;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Extensions;

namespace AMKDownloadManager.BatchDownloadFileSupport.AddIn
{
    [Export(typeof(IComponent))]
    public class BatchDownloadFileSupportComponent : IComponent
    {
        public string Name { get; }
        public string Description { get; }
        public string Author { get; }
        public Version Version { get; }
        
        
        
        public void Install(IApplicationContext application)
        {
            
        }

        public void Uninstall(IApplicationContext application)
        {
            
        }

        public void Initialize(IApplicationContext application)
        {
            
        }

        public void AfterInitialize(IApplicationContext application)
        {
            
        }

        public void Unload(IApplicationContext application)
        {
            
        }
    }
}
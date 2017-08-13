using System;
using AMKDownloadManager.Core.Extensions;
using System.Composition;
using AMKDownloadManager.Core.Api;
using System.Reflection;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;

namespace AMKDownloadManager.HttpDownloader.AddIn
{
    [Export(typeof(IComponent))]
    public class Component : IComponent
    {
        

        public Component()
        {
            
        }

        #region IComponent implementation

        public void Initialize(IAppContext app)
        {
            app.AddFeature(new HttpProtocolProvider());
        }

        public void Unload(IAppContext app)
        {
            app.RemoveFeature(new HttpProtocolProvider());
        }

        public string Name => "HttpDownloader";

        public string Description => "Download files over http(s)";

        public string Author => "Ali Mousavi Kherad";

        public Version Version
        {
            get
            {
                var assembly = typeof(Component).GetTypeInfo().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
        }

        #endregion
    }
}
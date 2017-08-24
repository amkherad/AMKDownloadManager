using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Extensions;
using ir.amkdp.gear.core.Trace;

namespace AMKDownloadManager.Defaults
{
    public class ComponentImporter
    {
        [ImportMany]
        public IEnumerable<IComponent> Components { get; set; }
        
        
        public void Compose()
        {
            var executableLocation = Assembly.GetEntryAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(executableLocation), "Plugins");
            var assemblies = Directory
                .GetFiles(path, "*.dll", SearchOption.AllDirectories)
                .Select(Assembly.LoadFile)
                .ToList();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);
            using (var container = configuration.CreateContainer())
            {
                Components = container.GetExports<IComponent>();
            }
        }

        public int AvailableNumberOfComponents => Components.Count();

        public void InitializeAll(IAppContext appContext)
        {
            if (Components != null)
            {
                foreach (var com in Components)
                {
                    Logger.Write(com.Description);
                    com.Initialize(appContext);
                }
            }
        }
    }
}
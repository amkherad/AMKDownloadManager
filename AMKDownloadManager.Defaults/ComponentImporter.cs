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
    /// <summary>
    /// Generic component loader using <see cref="System.Composition"/>.
    /// </summary>
    public class ComponentImporter
    {
        /// <summary>
        /// All loaded components.
        /// </summary>
        [ImportMany]
        public IEnumerable<IComponent> Components { get; set; }
        
        /// <summary>
        /// Load plugins from './Plugins' directory.
        /// </summary>
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

        /// <summary>
        /// Number of total components loaded.
        /// </summary>
        /// <remarks>Equivalent to <see cref="Components.Count()"/></remarks>
        public int NumberOfAvailableComponents => Components.Count();

        /// <summary>
        /// Calls <see cref="IComponent.Initialize"/> on all loaded components.
        /// </summary>
        /// <param name="appContext"></param>
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
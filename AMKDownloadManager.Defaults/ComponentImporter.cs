using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Extensions;
using AMKsGear.Core.Trace;

namespace AMKDownloadManager.Defaults
{
    /// <summary>
    /// Generic component loader using <see cref="System.Composition"/>.
    /// </summary>
    public class ComponentImporter
    {
        public string ProbingPattern { get; set; } = "*.plugin.dll";

        /// <summary>
        /// All loaded components.
        /// </summary>
        [ImportMany]
        public IEnumerable<IComponent> Components { get; set; }

        /// <summary>
        /// Load plugins from given directories.
        /// </summary>
        public void Compose(string[] paths)
        {
            var files = new Dictionary<string, string>();

            foreach (var path in paths)
            {
                //try
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var file in Directory.GetFiles(path, ProbingPattern, SearchOption.AllDirectories))
                        {
                            var key = Path.GetFileName(file);
                            if (!files.ContainsKey(key))
                            {
                                files.Add(key, file);
                            }
                        }
                    }
                }
                //catch { }
            }

            var assemblies = files.Values
                .Select(assembly =>
                {
                    try
                    {
                        return Assembly.LoadFile(assembly);
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.Log(ex);
                        return null;
                    }
                })
                .Where(assembly => assembly != null)
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
        /// <param name="applicationContext"></param>
        public void InitializeAll(IApplicationContext applicationContext)
        {
            if (Components != null)
            {
                foreach (var com in Components)
                {
                    com.Initialize(applicationContext);
                }

                foreach (var com in Components)
                {
                    com.AfterInitialize(applicationContext);
                }
            }
        }
    }
}
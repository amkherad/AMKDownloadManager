using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

        private IEnumerable<string> GetAllPluginFiles(string path)
        {
            var fx = RuntimeInformation.FrameworkDescription.Split(' ');

            if (fx.Length == 0)
            {
                return Directory.GetFiles(path, ProbingPattern, SearchOption.AllDirectories);
            }

            IEnumerable<string> validLeftMatches;
            IEnumerable<string> invalidLeftMatches;

            var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

            if (fx[0].ToUpper() == ".NET")
            {
                if (fx.Length == 1)
                {
                    return Directory.GetFiles(path, ProbingPattern, SearchOption.AllDirectories);
                }

                switch (fx[1].ToLower())
                {
                    case "core":
                    {
                        validLeftMatches = new[] {"netcoreapp*.", "netstandard*.", arch};
                        invalidLeftMatches = new[] {"net#*", "x64", "x86", "arm", "arm64"}
                            .Where(x => x != arch);
                        break;
                    }
                    case "framework":
                    {
                        validLeftMatches = new[] {"net#*", "netstandard*.", arch};
                        invalidLeftMatches =
                            new[] {"netcoreapp*.", "x64", "x86", "arm", "arm64"}
                                .Where(x => x != arch);
                        break;
                    }
                    case "native":
                    {
                        validLeftMatches = new[] {arch};
                        invalidLeftMatches = new[] {"net#*", "x64", "x86", "arm", "arm64"}
                            .Where(x => x != arch);
                        break;
                    }
                    default:
                    {
                        return Directory.GetFiles(path, ProbingPattern, SearchOption.AllDirectories);
                    }
                }
            }
            else //maybe mono?
            {
                validLeftMatches = new[] {"net#*", "netstandard*.", arch};
                invalidLeftMatches = new[] {"netcoreapp*.", "x64", "x86", "arm", "arm64"}
                    .Where(x => x != arch);
            }

            var validLeftMatchesRegex = validLeftMatches.Select(x => new Regex(x)).ToArray();
            var invalidLeftMatchesRegex = invalidLeftMatches.Select(x => new Regex(x)).ToArray();

            var result = new List<string>();
            foreach (var file in Directory.GetFiles(path, ProbingPattern, SearchOption.AllDirectories))
            {
                var dir = Path.GetFileName(Path.GetDirectoryName(file));

                if (!invalidLeftMatchesRegex.Any(invalid => invalid.IsMatch(dir))
                    /*&& validLeftMatchesRegex.Any(valid => valid.IsMatch(dir))*/ )
                {
                    result.Add(file);
                }
            }

            return result;
        }

        /// <summary>
        /// Load plugins from given directories.
        /// </summary>
        public void Compose(string[] paths)
        {
            //used to import one out of many with the same name.
            var files = new Dictionary<string, string>();

            foreach (var path in paths)
            {
                //try
                {
                    if (Directory.Exists(path))
                    {
                        var pluginFiles = GetAllPluginFiles(path);
                        foreach (var file in pluginFiles)
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
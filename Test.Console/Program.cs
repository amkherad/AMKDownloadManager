using System;
using System.Linq;
using System.Reflection;
using ir.amkdp.gear.arch.Trace.Annotations;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.HttpDownloader.AddIn;
using ir.amkdp.gear.core.Trace;
using ir.amkdp.gear.core.Trace.LoggerEngines;

namespace Test.Console
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Logger.RegisterLogger(new MethodLogger(System.Console.Write, f =>
            {
                for (var i = 0; i < f; i++) System.Console.WriteLine();
            }));

            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            AppHelpers.InjectTopLayerFeatures(pool);
            //AppHelpers.LoadComponents(app);
            var component = new Component();
            component.Initialize(pool);
            AppHelpers.ConfigureFeatures(pool);

            var instances = Assembly.GetExecutingAssembly().DefinedTypes
                .Select(x =>
                    new
                    {
                        Class = x,
                        CustomAttributes = x.GetCustomAttributes().OfType<TestClassAttribute>(),
                    })
                .Where(x => x.CustomAttributes.Any())
                .OrderBy(x => x.CustomAttributes.First().Order)
                .Select(x => Activator.CreateInstance(x.Class));

            Trace.WriteLine("");
            foreach (var instance in instances)
            {
                var cls = instance.GetType();
                var methods = cls.GetMethods().Where(x => x.GetCustomAttributes().Any(c => c is TestMethodAttribute));

                var sw = new Stopwatch();

                foreach (var method in methods)
                {
                    Trace.WriteLine($"[TestMethod: {method.Name}()]");
                    sw.Start();
                    try
                    {
                        method.Invoke(instance, null);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Method {method.Name} Exception: {ex}");
                    }
                    sw.Stop();

                    Trace.WriteLine(
                        "----------------------------" + Environment.NewLine +
                        $"Method {method.Name} of class {cls.Name} took {sw.ElapsedMilliseconds}ms ({sw.ElapsedTicks} ticks)" + Environment.NewLine +
                        "================================================================================================================");
                }
            }

            ApplicationHost.Instance.Unload();

            System.Console.ReadKey();
        }
    }
}
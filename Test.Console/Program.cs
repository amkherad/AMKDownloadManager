using System;
using System.Linq;
using System.Reflection;
using ir.amkdp.gear.arch.Trace.Annotations;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Threading;
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

            var app = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            AMKDownloadManager.MainClass.InjectTopLayerFeatures(app);
            AMKDownloadManager.MainClass.LoadComponents(app);

            var classes = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(x => x.IsClass &&
                    x.GetCustomAttributes().Any(c => c is TestClassAttribute));
            
            foreach (var cls in classes)
            {
                var instance = Activator.CreateInstance(cls);
                var methods = cls.GetMethods().Where(x => x.GetCustomAttributes().Any(c => c is TestMethodAttribute));

                var sw = new Stopwatch();

                foreach (var method in methods)
                {
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

                    Trace.WriteLine($"Method {method.Name} of class {cls.Name} took {sw.ElapsedMilliseconds}ms ({sw.ElapsedTicks} ticks)");
                }
            }

            ApplicationHost.Instance.Unload();

            System.Console.ReadKey();
        }
    }
}
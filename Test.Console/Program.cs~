using System;
using System.Linq;
using System.Reflection;
using ir.amkdp.gear.arch.Trace.Annotations;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Threading;

namespace Test.Console
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            ApplicationHost.Instance.Initialize(new AbstractThreadFactory());

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
                    method.Invoke(instance, null);
                    sw.Stop();

                    Trace.WriteLine($"Method {method.Name} of class {cls.Name} took {sw.ElapsedMilliseconds}ms ({sw.ElapsedTicks} ticks)");
                }
            }

            ApplicationHost.Instance.Unload();

            System.Console.ReadKey();
        }
    }
}
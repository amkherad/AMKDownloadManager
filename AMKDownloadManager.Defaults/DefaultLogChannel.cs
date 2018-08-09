using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Architecture.Trace;

namespace AMKDownloadManager.Defaults
{
    public class DefaultLoggerChannel : ILogger
    {
        private IProducerConsumerCollection<string> _logs = new ConcurrentQueue<string>();

        public event EventHandler LogAvailable;


        public void Dispose()
        {
        }

        public void LogString(string @string,
            ILoggingContext context,
            string callerMemberName = null,
            int callerLineNumber = 0,
            string callerFilePath = null)
        {
            _logs.TryAdd(@string);
            LogAvailable?.Invoke(this, EventArgs.Empty);
        }

        public void LogException(Exception exception,
            ILoggingContext context,
            string callerMemberName = null,
            int callerLineNumber = 0,
            string callerFilePath = null)
        {
            _logs.TryAdd(exception.ToString());
            LogAvailable?.Invoke(this, EventArgs.Empty);
        }

        
        /// <summary>
        /// It may return an empty enumerable.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> TakeLogs()
        {
            //it will be empty if another thread already has taken the item.
            while (_logs.TryTake(out var item))
            {
                yield return item;
            }
        }

        public int Order => 0;
        
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
        }
    }
}
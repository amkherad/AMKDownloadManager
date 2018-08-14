using System;
using System.Diagnostics;
using System.Threading;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Threading;
using AMKDownloadManager.Defaults.Messaging;
using AMKDownloadManager.Defaults.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.InterProcessTesting
{
    [TestClass]
    public class HubServerTests
    {
        HubServer _hub;
        HubClient _client;
        
        [TestMethod]
        public void TestClientToServerConnection()
        {
            var appContext = ApplicationHost.Instance.Pool;

            var ipcLock = new InterProcessLockService(appContext);
            var threadFactory = appContext.GetFeature<IThreadFactory>();

            ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
            
            Trace.WriteLine($"workerThreads:{workerThreads}, completionPortThreads:{completionPortThreads}");
            
            Trace.WriteLine("===============Testing==============");

            
            appContext.ScheduleBackgroundTask("jack", () =>
            {
                _hub = new HubServer("TESTHub1", "TESTHub1", ipcLock, threadFactory);

                Trace.WriteLine("Server hub created.");

                _hub.DataReceived += (sender, args) =>
                {
                    Trace.WriteLine($"Message received in server: {args.Buffer}");
                };
                _hub.ClientConnected += (sender, args) =>
                {
                    Trace.WriteLine($"Client connected: {args.ProcessId}");
                };
                _hub.ClientDisconnected += (sender, args) =>
                {
                    Trace.WriteLine($"Client disconnected: {args.ProcessId}");
                };

                Trace.WriteLine("Server listening.");
                _hub.Listen();
            });

            appContext.ScheduleBackgroundTask("jack", () =>
            {
                _client = new HubClient("TESTHub1", threadFactory);

                Trace.WriteLine("Client created.");

                _client.DataReceived += (sender, args) =>
                {
                    Trace.WriteLine($"Message received in client: {args.Buffer}");
                };
                _client.ServerConnected += (sender, args) =>
                {
                    Trace.WriteLine($"Server connected: {args.ProcessId}");
                };
                _client.ServerDisconnected += (sender, args) =>
                {
                    Trace.WriteLine($"Server disconnected: {args.ProcessId}");
                };

                Trace.WriteLine("Client connecting.");

                _client.Connect();
            });

            SpinWait.SpinUntil(() => _hub != null && _client != null && _client.IsConnected);

            _client.Send("Hello from client");
            
            _hub.Send("Hello from hub");
        }
    }
}
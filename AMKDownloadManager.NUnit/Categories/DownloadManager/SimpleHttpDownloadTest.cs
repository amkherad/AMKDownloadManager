using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Transport;
using ir.amkdp.gear.core.Trace;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit.Categories.DownloadManager
{
    [TestFixture]
    public class SimpleHttpDownloadTest
    {
        [Test]
        public void SendSimpleRequestToGoogle()
        {
            Logger.WriteLine("Test");
            try
            {
                //var di = DownloadBuilder.FromUri(new Uri("http://localhost:8080/VBoxGuestAdditions.iso")); //index.php
                var di = DownloadBuilder.FromUri(
                    new Uri(
                        "http://localhost:8081/downloads/Metal Gear Solid V - The Phantom Pain ''Nuclear'' Lyrics [HD].mp4")
                ); //index.php
                
                di.HttpProxy = new HttpProxyDescriptor(40985);
                
                var app = ApplicationHost.Instance.Pool;
                var downloadManager = app.GetFeature<IDownloadManager>();
                var fileProvider = app.GetFeature<IFileProvider>();

                var protocol = DownloadBuilder.Bind(di, app);
                var job = protocol.CreateJob(app, di, fileProvider, null);

                var state = downloadManager.Schedule(job);

                downloadManager.Start();
                downloadManager.Join();
            }
            catch (ThreadAbortException e)
            {
                Debugger.Break();
            }
            //state.
        }
    }
}
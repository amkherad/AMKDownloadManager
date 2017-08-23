﻿using System;
using ir.amkdp.gear.arch.Trace.Annotations;
using System.Diagnostics;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace Test.Console.Tests
{
    [TestClass]
    public class SimpleHttpDownloadTest
    {
        public SimpleHttpDownloadTest()
        {
        }

        [TestMethod]
        public void SendSimpleRequestToGoogle()
        {
            Trace.WriteLine("SendSimpleRequestToGoogle => Testing");

            var di = DownloadBuilder.FromUri(new Uri("http://google.com/"));

            var app = ApplicationHost.Instance.Pool;
            var downloadManager = app.GetFeature<IDownloadManager>();

            var protocol = DownloadBuilder.Bind(di, app);
            var job = protocol.CreateJob(app, di, null);

            var state = downloadManager.Schedule(job);

            downloadManager.Start();
            //state.
        }
    }
}
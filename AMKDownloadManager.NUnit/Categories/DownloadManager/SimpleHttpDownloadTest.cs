using System;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit.Categories.DownloadManager
{
    [TestFixture]
    public class SimpleHttpDownloadTest
    {
        [Test]
        public void SendSimpleRequestToGoogle()
        {
            Trace.WriteLine("SendSimpleRequestToGoogle => Testing");

            //var di = DownloadBuilder.FromUri(new Uri("http://localhost:8080/VBoxGuestAdditions.iso")); //index.php
            var di = DownloadBuilder.FromUri(new Uri("http://localhost:8081/downloads/VBoxGuestAdditions.iso")); //index.php

            var app = ApplicationHost.Instance.Pool;
            var downloadManager = app.GetFeature<IDownloadManager>();

            var protocol = DownloadBuilder.Bind(di, app);
            var job = protocol.CreateJob(app, di, null);

            var state = downloadManager.Schedule(job);

            downloadManager.Start();
            downloadManager.Join();
            //state.
        }
    }
}
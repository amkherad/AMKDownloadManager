using System;
using System.DiDataTestMethodagnostics;
using System.Threading;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.Categories.DownloadManager
{
    [TestClass]
    public class SimpleHttpDownloadTest
    {
        [TestMethod]
        public void SendSimpleRequestToGoogle()
        {
            try
            {
                //var di = DownloadBuilder.FromUri(new Uri("http://localhost:8080/VBoxGuestAdditions.iso")); //index.php
                var di = DownloadBuilder.FromUri(
                    new Uri(GeneralTests.Uri)
                );
                di.DebugName = "com.playstation.U4.apk";
                var di2 = DownloadBuilder.FromUri(
                    new Uri("http://localhost.:8084/CV.pdf")
                );
                di2.DebugName = "CV.pdf";
                
                //di.HttpProxy = new HttpProxyDescriptor(33849)
                //{
                //    BypassOnLocal = false
                //}; //http debugger port
                
                var app = ApplicationHost.Instance.Pool;
                var downloadManager = app.GetFeature<IDownloadManager>();
                var fileProvider = app.GetFeature<IFileProvider>();

                var protocol = DownloadBuilder.Bind(di, app);
                
                var job = protocol.CreateJob(app, di, fileProvider, null);
                var job2 = protocol.CreateJob(app, di2, fileProvider, null);

                downloadManager.Schedule(job);
                downloadManager.Schedule(job2);
                
                downloadManager.Start();
                downloadManager.WaitToFinish();
            }
            catch (ThreadAbortException e)
            {
                Debugger.Break();
            }
            //state.
        }
    }
}
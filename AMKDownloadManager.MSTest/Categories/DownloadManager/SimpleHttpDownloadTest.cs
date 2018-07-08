using System;
using System.Diagnostics;
using System.Threading;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Transport;
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
                ); //index.php
                
                //di.HttpProxy = new HttpProxyDescriptor(33849)
                //{
                //    BypassOnLocal = false
                //}; //http debugger port
                
                var app = ApplicationHost.Instance.Pool;
                var downloadManager = app.GetFeature<IDownloadManager>();
                var fileProvider = app.GetFeature<IFileProvider>();

                var protocol = DownloadBuilder.Bind(di, app);
                var job = protocol.CreateJob(app, di, fileProvider, null);

                var state = downloadManager.Schedule(job);
                
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
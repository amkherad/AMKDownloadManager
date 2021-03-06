﻿using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Defaults.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.Categories.FileSystem
{
    [TestClass]
    public class DefaultFileProviderTests
    {
        [TestMethod]
        public void TryReserveFileNameTest()
        {
            var fileProvider = new DefaultFileProvider(ApplicationContext.Instance as IApplicationContext);

            Assert.AreEqual(
                "/etc/host_2",
                fileProvider.TryReserveFileName("/etc/host")
            );
            
            Assert.AreEqual(
                "/etc/amk_psedu_file",
                fileProvider.TryReserveFileName("/etc/amk_psedu_file")
            );
        }
    }
}
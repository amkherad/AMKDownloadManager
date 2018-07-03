using System;
using System.IO;
using AMKDownloadManager.Defaults.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.Categories.FileSystem
{
    [TestClass]
    public class DefaultDownloadPathProviderTests
    {
        [TestMethod]
        public void Test1()
        {
            var pp = new DefaultDownloadPathProvider();
            
            Assert.AreEqual(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads"),
                pp.ReplacePath("[Downloads]"),
                "Downloads");
            
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                pp.ReplacePath("[Documents]"),
                "Documents");
            
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                pp.ReplacePath("[Musics]"),
                "Musics");
            
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                pp.ReplacePath("[Pictures]"),
                "Pictures");
            
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                pp.ReplacePath("[Videos]"),
                "Videos");
            
            Assert.AreEqual(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Applications"),
                pp.ReplacePath("[Applications]"),
                "Applications");
            
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                pp.ReplacePath("[AppData]"),
                "AppData");
        }
    }
}
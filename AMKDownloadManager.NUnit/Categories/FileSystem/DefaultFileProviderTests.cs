using AMKDownloadManager.Defaults.FileSystem;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit.Categories.FileSystem
{
    [TestFixture]
    public class DefaultFileProviderTests
    {
        [Test]
        public void TryReserveFileNameTest()
        {
            var fileProvider = new DefaultFileProvider(2);

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
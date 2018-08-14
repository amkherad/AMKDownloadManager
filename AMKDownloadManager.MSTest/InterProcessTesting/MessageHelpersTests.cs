using System.Diagnostics;
using AMKDownloadManager.Defaults.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.InterProcessTesting
{
    [TestClass]
    public class MessageHelpersTests
    {
        [TestMethod]
        public void TestEscapeAndUnEscapeCrLf()
        {
            TestText("Hello\r\nWorld", @"Hello\r\nWorld");
            
            TestText("Hello\r\n\\World", @"Hello\r\n\\World");
            
            TestText("\\\\\rHello\\\r\n\\World\\", @"\\\\\rHello\\\r\n\\World\\");

            var result = MessageHelpers.UnEscapeCrLf("\\\\Hello\\nWorldnr");
            Assert.AreEqual("\\Hello\nWorldnr", result);
        }
        

        private void TestText(string input, string expectation)
        {
            var result = MessageHelpers.EscapeCrLf(input);

            Trace.WriteLine($"EscapeCrLf resulted in: {result}");
            
            Assert.AreEqual(expectation, result);

            result = MessageHelpers.UnEscapeCrLf(expectation);
            
            Trace.WriteLine($"UnEscapeCrLf resulted in: {result}");
            
            Assert.AreEqual(input, result);
        }
    }
}
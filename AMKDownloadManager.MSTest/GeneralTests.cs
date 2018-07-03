using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AMKsGear.Core.Trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest
{
    [TestClass]
    public class GeneralTests
    {
        public const string Uri = "http://localhost.:8083/com.playstation.U4.apk";
        
        [TestMethod]
        public void Exec()
        {
            
        }
        [TestMethod]
        public void InspectHttpBehaviorOnHttpWebRequest()
        {
            var req = WebRequest.CreateHttp(Uri);
            
            var proxy = new WebProxy(new Uri("http://localhost.:33849/"))
            {
                BypassProxyOnLocal = false
            };
            req.Proxy = proxy;
            var isBypassed = proxy.IsBypassed(new Uri(Uri));
            Logger.Write(isBypassed);
            
            using (var response = req.GetResponse())
            {
                Logger.Write(response.ContentLength);
            }

            Debugger.Break();
        }
        
        [TestMethod]
        public void InspectHttpBehaviorOnBareSocket()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                var payload = new StringBuilder(256);
                payload.Append("GET /com.playstation.U4.apk HTTP/1.1\r\n");
                payload.Append("Host: localhost\r\n");
                payload.Append("Connection: keep-alive\r\n");
                payload.Append("\r\n");

                socket.Connect(IPAddress.Loopback, 8083);
                socket.Send(Encoding.UTF8.GetBytes(payload.ToString()));
                
                using (var stream = new NetworkStream(socket))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var headers = new StringBuilder(256);
                        string buf;
                        while ((buf = reader.ReadLine()) != string.Empty)
                        {
                            headers.AppendLine(buf);
                        }

                        headers.AppendLine();

                        Trace.WriteLine(headers.ToString());
                        
                        Debugger.Break();
                    }
                }
            }
        }
        
    }
}
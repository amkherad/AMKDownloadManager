using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit
{
    [TestFixture]
    public class GeneralTests
    {
        
        
        [Test]
        public void Exec()
        {
            
        }
        [Test]
        public void InspectHttpBehavior()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                var payload = new StringBuilder(256);
                payload.Append("GET /downloads/VBoxGuestAdditions.iso HTTP/1.1\r\n");
                payload.Append("Host: localhost\r\n");
                payload.Append("Connection: keep-alive\r\n");
                payload.Append("\r\n");

                socket.Connect(IPAddress.Loopback, 8081);
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
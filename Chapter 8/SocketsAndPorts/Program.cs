using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketsAndPorts {
    class Program {
        static void Main(string[] args) {
            string server = "localhost";
            int port = 5000;
            string path = "/api/values";

            Socket socket = null;
            IPEndPoint endpoint = null;
            var host = Dns.GetHostEntry(server);

            foreach (var address in host.AddressList) {
                socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                endpoint = new IPEndPoint(address, port);
                socket.ConnectAsync(endpoint).Wait();
                if (socket.Connected) {
                    break;
                }
            }

            var message = GetRequestMessage(server, port, path);
            var messageBytes = Encoding.ASCII.GetBytes(message);
            var segment = new ArraySegment<byte>(messageBytes);

            socket.SendAsync(segment, SocketFlags.None).Wait();

            var receiveSeg = new ArraySegment<byte>(new byte[512], 0, 512);

            socket.ReceiveAsync(receiveSeg, SocketFlags.None).Wait();

            string receivedMessage = Encoding.ASCII.GetString(receiveSeg);

            foreach (var line in receivedMessage.Split("\r\n")) {
                Console.WriteLine(line);
            }
            
            socket.Disconnect(false);
            socket.Dispose();
            Thread.Sleep(10000);
        }

        private static string GetRequestMessage(string server, int port, string path) {
            var message = $"GET {path} HTTP/1.1\r\n";
            message += $"Host: {server}:{port}\r\n";
            message += "cache-control: no-cache\r\n";
            message += "\r\n";
            return message;
        }
    }
}

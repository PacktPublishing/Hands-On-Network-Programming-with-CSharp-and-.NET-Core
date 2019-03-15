using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleUdpClient {
    class Program {
        public static async Task Main(string[] args) {
            using (var client = new UdpClient(34567)) {
                var remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 45678);

                client.Connect(remoteEndpoint);

                var message = "Testing UDP";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(messageBytes, messageBytes.Length);

                var response = await client.ReceiveAsync();
                var responseMessage = Encoding.UTF8.GetString(response.Buffer);
                Console.WriteLine(responseMessage);

                Thread.Sleep(10000);
            }
        }
    }
}

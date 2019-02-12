using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleTcpClient {
    class Program {
        public static async Task Main(string[] args) {
            int port = 54321;
            IPAddress address = IPAddress.Parse("127.0.0.1");
            var messages = new string[] {
              "Hello server | Return this payload to sender!",
              "To the server | Send this payload back to me!",
              "Server Header | Another returned message.",
              "Header Value | Payload to be returned",
              "TERMINATE"
            };
            var i = 0;
            while (i < messages.Length) {
                using (TcpClient client = new TcpClient()) {
                    client.Connect(address, port);
                    if (client.Connected) {
                        Console.WriteLine("We've connected from the client");
                    }

                    var bytes = Encoding.UTF8.GetBytes(messages[i++]);
                    using (var requestStream = client.GetStream()) {
                        await requestStream.WriteAsync(bytes, 0, bytes.Length);
                        var responseBytes = new byte[256];
                        await requestStream.ReadAsync(responseBytes, 0, responseBytes.Length);
                        var responseMessage = Encoding.UTF8.GetString(responseBytes);
                        Console.WriteLine();
                        Console.WriteLine("Response Received From Server:");
                        Console.WriteLine(responseMessage);
                    }
                }
                var sleepDuration = new Random().Next(2000, 10000);
                Console.WriteLine($"Generating a new request in {sleepDuration / 1000} seconds");
                Thread.Sleep(sleepDuration);
            }
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;

namespace SshDemo {
    class Program {
        public static async Task Main(string[] args) {
            AuthenticationMethod method = new PasswordAuthenticationMethod("tuxbox", "fiddlesticks");
            ConnectionInfo connection = new ConnectionInfo("127.0.0.1", "tuxbox", method);
            var client = new SshClient(connection);
            if (!client.IsConnected) {
                Console.WriteLine("We're connected!");
                client.Connect();
            }
            var readCommand = client.RunCommand("uname -mrs");
            Console.WriteLine(readCommand.Result);
            var writeCommand = client.RunCommand("mkdir \"/home/tuxbox/Desktop/ssh_output\"");
            Thread.Sleep(10000);
        }
    }
}

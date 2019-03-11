using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorHandling {
    public class Program {
        public static async Task Main(string[] args) {
            var test = await AsyncDemo.AsyncMethodDemo();
            for (var i = 0; i < 24; i++) {
                Console.WriteLine($"Polly Demo Attempt {i}");
                Console.WriteLine("-------------");
                PollyDemo.ExecuteRemoteLookupWithPolly();
                Console.WriteLine("-------------");
                Thread.Sleep(5000);
            }
        }
    }
}

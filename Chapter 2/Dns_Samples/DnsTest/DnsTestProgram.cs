using System;
using System.Net;
using System.Threading;

namespace DnsTest
{
    class DnsTestProgram {
        static void Main(string[] args) {
            var domainEntry = Dns.GetHostEntry("fun.with.dns.com");
            Console.WriteLine(domainEntry.HostName);
            foreach (var ip in domainEntry.AddressList) {
                Console.WriteLine(ip);
            }

            var domainEntryByAddress = Dns.GetHostEntry("127.0.0.1");
            Console.WriteLine(domainEntryByAddress.HostName);
            foreach (var ip in domainEntryByAddress.AddressList) {
                Console.WriteLine(ip);
            }
            Thread.Sleep(10000);
        }
    }
}

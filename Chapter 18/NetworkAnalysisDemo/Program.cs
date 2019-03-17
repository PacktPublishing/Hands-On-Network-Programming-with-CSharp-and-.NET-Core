using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetworkAnalysisDemo {
    class Program {
        static void Main(string[] args) {
            DisplayDeviceInformation();
            Console.ReadLine();
            DisplayActiveTcpConnections();
            Console.ReadLine();
            DisplayIPv4TrafficStatistics();
            Console.ReadLine();
            Thread.Sleep(20000);
        }

        private static void DisplayDeviceInformation() {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine($"There were {adapters.Length} devices detected on your machine");
            Console.WriteLine();
            Console.WriteLine("Device Details");
            foreach (NetworkInterface adapter in adapters) {
                Console.WriteLine("=========================================================================");
                Console.WriteLine();
                Console.WriteLine($"Device ID: ----------------- {adapter.Id}");
                Console.WriteLine($"Device Name: --------------- {adapter.Name}");
                Console.WriteLine($"Description: --------------- {adapter.Description}");
                Console.WriteLine($"Interface type: ------------ {adapter.NetworkInterfaceType}");
                Console.WriteLine($"Physical Address: ---------- {adapter.GetPhysicalAddress().ToString()}");
                Console.WriteLine($"Operational status: -------- {adapter.OperationalStatus}");
                Console.WriteLine($"Adapter Speed: ------------- {adapter.Speed}");
                Console.WriteLine($"Multicast Support: --------- {adapter.SupportsMulticast}");
            }

        }

        private static void DisplayActiveTcpConnections() {
            var ipStats = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnections = ipStats.GetActiveTcpConnections();

            Console.WriteLine($"There are currently {tcpConnections.Length} active TCP connections on this machine");
            Console.WriteLine();

            foreach (var connection in tcpConnections) {
                Console.WriteLine("=============================================");
                Console.WriteLine($"Local host:");
                Console.WriteLine($"    Connected On Address: {connection.LocalEndPoint.Address.ToString()}");
                Console.WriteLine($"    Over Port Number: {connection.LocalEndPoint.Port}");
                Console.WriteLine($"Remote host: {connection.RemoteEndPoint.Address}");
                Console.WriteLine($"    Connected On Address: {connection.RemoteEndPoint.Address.ToString()}");
                Console.WriteLine($"    Over Port Number: {connection.RemoteEndPoint.Port}");
                Console.WriteLine($"Connection State: {connection.State.ToString()}");
            }
        }

        private static void DisplayIPv4TrafficStatistics() {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipStats = ipProperties.GetIPv4GlobalStatistics();
            Console.WriteLine($"Incoming Packets: {ipStats.ReceivedPackets}");
            Console.WriteLine($"Outgoing Packets: {ipStats.OutputPacketRequests}");
            Console.WriteLine($"Discareded Incoming Packets: {ipStats.ReceivedPacketsDiscarded}");
            Console.WriteLine($"Discarded Outgoing Packets: {ipStats.OutputPacketsDiscarded}");
            Console.WriteLine($"Fragmentation Failures: {ipStats.PacketFragmentFailures}");
            Console.WriteLine($"Reassembly Failures: {ipStats.PacketReassemblyFailures}");
        }
    }
}

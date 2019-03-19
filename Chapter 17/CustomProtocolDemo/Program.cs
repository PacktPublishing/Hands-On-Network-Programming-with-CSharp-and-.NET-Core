using System;
using System.Net;
using System.Threading.Tasks;

namespace CustomProtocolDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            WebRequest.RegisterPrefix("dtrj", new RockWebRequestCreator());

            Console.WriteLine("Hello World!");
        }
    }
}

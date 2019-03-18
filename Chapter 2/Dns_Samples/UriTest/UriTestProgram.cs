using System;
using System.Threading;

namespace UriTest
{
    public class UriTestProgram
    {
        public static Uri GetSimpleUri() {
            var builder = new UriBuilder();
            builder.Scheme = "http";
            builder.Host = "packt.com";
            return builder.Uri;
        }

        public static Uri GetSimpleUri_Constructor() {
            var builder = new UriBuilder("http", "packt.com");
            return builder.Uri;
        }

        public static void Main(string[] args)
        {
            var simpleUri = GetSimpleUri();
            Console.WriteLine(simpleUri.ToString());
            // Expected output: http://packt.com

            var constructorUri = GetSimpleUri_Constructor();
            Console.WriteLine(constructorUri.ToString());
            // Expected output: http://packt.com

            Thread.Sleep(10000);
        }
    }
}

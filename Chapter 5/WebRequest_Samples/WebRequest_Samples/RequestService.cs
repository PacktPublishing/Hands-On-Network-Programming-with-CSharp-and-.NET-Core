using System.Text;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System;

namespace WebRequest_Samples
{
    public class RequestService
    {
        private static readonly string FINANCE_CONN_GROUP = "financial_connection";
        private static readonly string REAL_ESTATE_CONN_GROUP = "real_estate_connection";

        public static void Main(string[] args)
        {
            SubmitWikiRequest();
        }

        public static void SubmitRealEstateRequest() 
        {
            WebRequest req = WebRequest.Create("https://real-estate-detail.com/market/api");
            req.ConnectionGroupName = REAL_ESTATE_CONN_GROUP;
            var noCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            req.CachePolicy = noCachePolicy;
            req.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            req.Credentials = new NetworkCredential("test_user", "secure_and_safe_password");
            Stream reqStream = req.GetRequestStream();
            var messageString = "test";
            var messageBytes = Encoding.UTF8.GetBytes(messageString);
            reqStream.Write(messageBytes, 0, messageBytes.Length);
        }

        public static void SubmitWikiRequest()
        {
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create("https://en.wikipedia.org/wiki/Main_Page");
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
            }

            // Close the response.  
            response.Close();
        }
    }
}

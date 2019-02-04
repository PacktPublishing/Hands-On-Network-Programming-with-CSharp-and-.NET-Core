using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FtpSampleApp {
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine(await GetDirectoryListing());
            Console.WriteLine(await RequestFile());
            Console.WriteLine(await PushFile());
        }

        public static async Task<string> GetDirectoryListing() {
            StringBuilder strBuilder = new StringBuilder();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://localhost");
            req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            req.Credentials = new NetworkCredential("s_burns", "test_password");

            FtpWebResponse resp = (FtpWebResponse)await req.GetResponseAsync();

            using (var respStream = resp.GetResponseStream()) {
                using (var reader = new StreamReader(respStream)) {
                    strBuilder.Append(reader.ReadToEnd());
                    strBuilder.Append(resp.WelcomeMessage);
                    strBuilder.Append($"Request returned status:  {resp.StatusDescription}");
                }
            }
            return strBuilder.ToString();
        }

        public static async Task<string> RequestFile() {
            StringBuilder strBuilder = new StringBuilder();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://localhost/FitnessApp/Startup.cs");
            req.Method = WebRequestMethods.Ftp.DownloadFile;

            req.Credentials = new NetworkCredential("s_burns", "test_password");
            req.UsePassive = true;

            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse()) {

                using (var respStream = resp.GetResponseStream()) {
                    strBuilder.Append(resp.StatusDescription);
                    if (!File.Exists(@"../Copy_Startup.cs")) {
                        using (var file = File.Create(@"../Copy_Startup.cs")) {
                            //We only use this to create the file in the path if it doesn't exist.
                        }
                    }
                    using (var respReader = new StreamReader(respStream)) {
                        using (var fileWriter = File.OpenWrite(@"../Copy_Startup.cs")) {
                            using (var strWriter = new StreamWriter(fileWriter)) {
                                await strWriter.WriteAsync(respReader.ReadToEnd());
                            }
                        }
                    }
                }
            }

            return strBuilder.ToString();
        }

        public static async Task<string> PushFile() {
            StringBuilder strBuilder = new StringBuilder();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://localhost/Program.cs");
            req.Method = WebRequestMethods.Ftp.UploadFile;

            req.Credentials = new NetworkCredential("s_burns", "test_password");
            req.UsePassive = true;

            byte[] fileBytes;

            using (var reader = new StreamReader(@"Program.cs")) {
                fileBytes = Encoding.ASCII.GetBytes(reader.ReadToEnd());
            }

            req.ContentLength = fileBytes.Length;

            using (var reqStream = await req.GetRequestStreamAsync()) {
                await reqStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse()) {
                strBuilder.Append(resp.StatusDescription);
            }

            return strBuilder.ToString();
        }
    }
}

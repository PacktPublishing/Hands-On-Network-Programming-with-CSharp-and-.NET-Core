using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace StreamsAndAsync {


    public class Program {

        public static void Main(string[] args) {
            var res = AsyncMethodDemo().Result;
            ComplexModel testModel = new ComplexModel();
            string testMessage = JsonConvert.SerializeObject(testModel);

            using (Stream ioStream = new FileStream(@"../stream_demo_file.txt", FileMode.OpenOrCreate)) {
                using (StreamWriter sw = new StreamWriter(ioStream)) {
                    sw.Write(testMessage);
                    sw.BaseStream.Seek(10, SeekOrigin.Begin);
                    sw.Write(testMessage);
                }
            }

            Console.WriteLine("Done!");
            Thread.Sleep(10000);
        }

        public static async Task<ResultObject> AsyncMethodDemo() {
            ResultObject result = new ResultObject();
            WebRequest request = WebRequest.Create("http://you.com");
            request.Method = "POST";
            Stream reqStream = request.GetRequestStream();
           
            using (StreamWriter sw = new StreamWriter(reqStream)) {
                sw.Write("Our test data query");
            }
            var responseTask = request.GetResponseAsync();

            result.LocalResult = 27;
            var webResponse = await responseTask;
                
            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream())) {
                result.RequestResult = await sr.ReadToEndAsync();
            }

            return result;
        }
    }
}

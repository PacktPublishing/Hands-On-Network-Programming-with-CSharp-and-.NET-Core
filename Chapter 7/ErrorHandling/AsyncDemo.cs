using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace ErrorHandling {
    public static class AsyncDemo {
        public static async Task<ResultObject> AsyncMethodDemo() {
            ResultObject result = new ResultObject();
            WebRequest request = WebRequest.Create("http://test-domain.com");
            request.Method = "POST";
            Stream reqStream = request.GetRequestStream();

            using (StreamWriter sw = new StreamWriter(reqStream)) {
                sw.Write("Our test data query");
            }
            var responseTask = request.GetResponseAsync();

            result.LocalResult = 27;
            try {
                var webResponse = await responseTask;

                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream())) {
                    result.RequestResult = await sr.ReadToEndAsync();
                }
            } catch (WebException ex) {
                ProcessException(ex);
            }

            return result;
        }

        private static void ProcessException(WebException ex) {
            switch (ex.Status) {
                case WebExceptionStatus.ConnectFailure:
                case WebExceptionStatus.ConnectionClosed:
                case WebExceptionStatus.RequestCanceled:
                case WebExceptionStatus.PipelineFailure:
                case WebExceptionStatus.SendFailure:
                case WebExceptionStatus.KeepAliveFailure:
                case WebExceptionStatus.Timeout:
                    Console.WriteLine("We should retry connection attempts");
                    break;
                case WebExceptionStatus.NameResolutionFailure:
                case WebExceptionStatus.ProxyNameResolutionFailure:
                case WebExceptionStatus.ServerProtocolViolation:
                    Console.WriteLine("Prevent further attempts and notify consumers to check URL configurations");
                    break;
                case WebExceptionStatus.SecureChannelFailure:
                case WebExceptionStatus.TrustFailure:
                    Console.WriteLine("Authentication or security issue. Prompt for credentials and perhaps try again");
                    break;

            }
        }
    }
}

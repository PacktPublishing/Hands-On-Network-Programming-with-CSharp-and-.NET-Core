using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Polly;

namespace ErrorHandling {
    public static class PollyDemo {
        private static List<WebExceptionStatus> connectionFailure = new List<WebExceptionStatus>() {
            WebExceptionStatus.ConnectFailure,
            WebExceptionStatus.ConnectionClosed,
            WebExceptionStatus.RequestCanceled,
            WebExceptionStatus.PipelineFailure,
            WebExceptionStatus.SendFailure,
            WebExceptionStatus.KeepAliveFailure,
            WebExceptionStatus.Timeout
        };

        private static List<WebExceptionStatus> resourceAccessFailure = new List<WebExceptionStatus>() {
            WebExceptionStatus.NameResolutionFailure,
            WebExceptionStatus.ProxyNameResolutionFailure,
            WebExceptionStatus.ServerProtocolViolation
        };

        private static List<WebExceptionStatus> securityFailure = new List<WebExceptionStatus>() {
            WebExceptionStatus.SecureChannelFailure,
            WebExceptionStatus.TrustFailure
        };

        public static HttpResponseMessage ExecuteRemoteLookup() {
            var num = new Random().Next();
            if (num % 3 == 0) {
                Console.WriteLine("Breaking the circuit");
                throw new WebException("Name Resolution Failure", WebExceptionStatus.NameResolutionFailure);
            } else if (num % 4 == 0) {
                Console.WriteLine("Falling Back");
                throw new WebException("Security Failure", WebExceptionStatus.TrustFailure);
            } else if (num % 2 == 0) {
                Console.WriteLine("Retrying connections...");
                throw new WebException("Connection Failure", WebExceptionStatus.ConnectFailure);
            }
            return new HttpResponseMessage();
        }

        private static HttpResponseMessage GetAuthorizationErrorResponse() {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        public static void ExecuteRemoteLookupWithPolly() {
            Policy connFailurePolicy = Policy
                .Handle<WebException>(x => connectionFailure.Contains(x.Status))
                .RetryForever();

            Policy<HttpResponseMessage> authFailurePolicy = Policy<HttpResponseMessage>
                .Handle<WebException>(x => securityFailure.Contains(x.Status))
                .Fallback(() => GetAuthorizationErrorResponse());

            Policy nameResolutionPolicy = Policy
                .Handle<WebException>(x => resourceAccessFailure.Contains(x.Status))
                .CircuitBreaker(1, TimeSpan.FromMinutes(2));

            Policy intermediatePolicy = Policy
                .Wrap(connFailurePolicy, nameResolutionPolicy);

            Policy<HttpResponseMessage> combinedPolicies = intermediatePolicy
                .Wrap(authFailurePolicy);

            try {
                HttpResponseMessage resp = combinedPolicies.Execute(() => ExecuteRemoteLookup());
                if (resp.IsSuccessStatusCode) {
                    Console.WriteLine("Success!");
                } else if (resp.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                    Console.WriteLine("We have fallen back!");
                }
            } catch (WebException ex) {
                if (resourceAccessFailure.Contains(ex.Status)) {
                    Console.WriteLine("We should expect to see a broken circuit.");
                }
            }
        }

    }
}

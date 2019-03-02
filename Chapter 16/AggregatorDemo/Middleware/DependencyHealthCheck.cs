using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AggregatorDemo {
    public class DependencyHealthCheck : IHealthCheck {
        private readonly int DEGRADING_THRESHOLD = 2000;
        private readonly int UNHEALTHY_THRESHOLD = 5000;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken token = default(CancellationToken)
        ) {
            var httpClient = HttpClientFactory.Create();
            httpClient.BaseAddress = new Uri("https://localhost:33333");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/dependency/new-data");
            Stopwatch sw = Stopwatch.StartNew();
            var response = await httpClient.SendAsync(request);
            sw.Stop();
            var responseTime = sw.ElapsedMilliseconds;
            if (responseTime < DEGRADING_THRESHOLD) {
                return HealthCheckResult.Healthy("The dependent system is performing within acceptable parameters");
            } else if (responseTime < UNHEALTHY_THRESHOLD) {
                return HealthCheckResult.Degraded("The dependent system is degrading and likely to fail soon");
            } else {
                return HealthCheckResult.Unhealthy("The dependent system is unacceptably degraded. Restart.");
            }
        }
    }
}

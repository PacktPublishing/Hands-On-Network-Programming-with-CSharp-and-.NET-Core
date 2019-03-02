using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AggregatorDemo {

    public interface IDependentDataService {
        Task<string> GetDataResponse();
    }

    public class DependentDataService : IDependentDataService {
        private IHttpClientFactory _httpFactory;

        public DependentDataService(IHttpClientFactory factory) {
            _httpFactory = factory;
        }

        public async Task<string> GetDataResponse() {
            var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:33333");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/dependency/new-data");

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

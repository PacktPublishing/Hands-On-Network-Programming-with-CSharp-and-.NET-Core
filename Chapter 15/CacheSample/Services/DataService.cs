using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CacheSample {
    public interface IDataService {
        Task<string> GetStringValueById(string id);
        Task<IEnumerable<string>> GetStringListById(string id);
        Task<DataRecord> GetRecordById(string id);
        Task<IEnumerable<DataRecord>> GetRecordListById(string id);
    }

    public class DataService : IDataService {
        private IHttpClientFactory _httpFactory;

        public DataService(IHttpClientFactory httpFactory) {
            _httpFactory = httpFactory;
        }

        public async Task<string> GetStringValueById(string id) {
            return await GetResponseString($"api/data/value/{id}");
        }

        public async Task<IEnumerable<string>> GetStringListById(string id) {
            var respStr = await GetResponseString($"api/data/values/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<string>>(respStr);
        }

        public async Task<DataRecord> GetRecordById(string id) {
            var respStr = await GetResponseString($"api/data/record/{id}");
            return JsonConvert.DeserializeObject<DataRecord>(respStr);
        }

        public async Task<IEnumerable<DataRecord>> GetRecordListById(string id) {
            var respStr = await GetResponseString($"api/data/records/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<DataRecord>>(respStr);
        }

        private async Task<string> GetResponseString(string path) {
            var client = _httpFactory.CreateClient(Constants.DATA_CLIENT);
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}

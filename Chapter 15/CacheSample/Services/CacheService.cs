using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CacheSample {

    public interface ICacheService {
        Task<bool> HasCacheRecord(string id);
        Task<string> FetchString(string id);
        Task<T> FetchRecord<T>(string id);
        Task WriteString(string id, string value);
        Task WriteRecord<T>(string id, T record);
    }

    public class CacheService : ICacheService {
        IDistributedCache _cache;

        public CacheService(IDistributedCache cache) {
            _cache = cache;
        }

        public async Task<bool> HasCacheRecord(string id) {
            var record = await _cache.GetStringAsync(id);
            return record != null;
        }

        public async Task<string> FetchString(string id) {
            return await _cache.GetStringAsync(id);
        }

        public async Task<T> FetchRecord<T>(string id) {
            var record = await _cache.GetStringAsync(id);
            T result = JsonConvert.DeserializeObject<T>(record);
            return result;
        }

        public async Task WriteString(string id, string value) {
            DistributedCacheEntryOptions opts = new DistributedCacheEntryOptions() {
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };
            await _cache.SetStringAsync(id, value, opts);
        }

        public async Task WriteRecord<T>(string id, T record) {
            var value = JsonConvert.SerializeObject(record);
            DistributedCacheEntryOptions opts = new DistributedCacheEntryOptions() {
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };

            await _cache.SetStringAsync(id, value, opts);
        }

    }
}

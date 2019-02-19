using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DNSSimulation {
    public static class Hosts {
        public static IDictionary<string, IEnumerable<string>> Map;
        static Hosts() {
            try {
                using (var sr = new StreamReader("hosts.json")) {
                    var json = sr.ReadToEnd();
                    Map = JsonConvert.DeserializeObject<IDictionary<string, IEnumerable<string>>>(json);
                }
            } catch (Exception e) {
                throw e;
            }
        }
    }
}

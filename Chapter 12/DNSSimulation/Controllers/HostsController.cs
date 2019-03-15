using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace DNSSimulation {
    [Route("dns/[controller]")]
    [ApiController]
    public class HostsController : ControllerBase {
        [HttpPost]
        public string Post([FromBody] string domainName) {
            var uri = new UriBuilder(domainName).Uri;
            IEnumerable<string> ipAddressStrings;
            if (!Hosts.Map.TryGetValue(uri.Host, out ipAddressStrings)) {
                return GetSerializedIPAddresses(Dns.GetHostAddresses(uri.Host));
            }

            var addresses = new List<IPAddress>();
            foreach (var addressString in ipAddressStrings) {
                if (!IPAddress.TryParse(addressString, out var newAddress)) {
                    continue;
                }
                addresses.Add(newAddress);
            }
            return GetSerializedIPAddresses(addresses);
        }

        private string GetSerializedIPAddresses(IEnumerable<IPAddress> addresses) {
            var str = new StringBuilder("[");
            var firstInstance = true;
            foreach (var address in addresses) {
                if (!firstInstance) {
                    str.Append(",");
                } else {
                    firstInstance = false;
                }
                str.Append("{");
                str.Append($"\"Address\": {address.ToString()},");
                str.Append($"\"AddressFamily\": {address.AddressFamily},");
                str.Append($"\"IsIPv4MappedToIPv6\": {address.IsIPv4MappedToIPv6},");
                str.Append($"\"IsIPv6LinkLocal\": {address.IsIPv6LinkLocal},");
                str.Append($"\"IsIPv6Multicast\": {address.IsIPv6Multicast},");
                str.Append($"\"IsIPv6SiteLocal\": {address.IsIPv6SiteLocal},");
                str.Append($"\"IsIPv6Teredo\": {address.IsIPv6Teredo}");
                str.Append("}");
            }

            str.Append("]");
            return str.ToString();
        }
    }
}

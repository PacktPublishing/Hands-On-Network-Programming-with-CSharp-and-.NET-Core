using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace AuthSample {

    public static class UserVault {
        private static Dictionary<string, string> _users { get; set; }

        static UserVault() {
            try {
                using (var sr = new StreamReader("user_vault.json")) {
                    var json = sr.ReadToEnd();
                    _users = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
            } catch (Exception e) {
                throw e;
            }
        }

        public static bool ContainsCredentials(string userName, string password) {
            if (_users.ContainsKey(userName)) {
                string storedPassword;
                if (_users.TryGetValue(userName, out storedPassword)) {
                    return storedPassword.Equals(password);
                }
            }
            return false;
        }
    }
}
